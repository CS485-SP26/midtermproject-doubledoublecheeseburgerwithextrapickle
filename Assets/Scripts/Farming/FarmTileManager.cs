using Core;
using Environment;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Farming
{
    public class FarmTileManager : MonoBehaviour
    {
        [SerializeField] private GameObject farmTilePrefab;
        [SerializeField] DayController dayController;
        [SerializeField] private int rows = 4;
        [SerializeField] private int cols = 4;
        [SerializeField] private float tileGap = 0.1f;

        private List<FarmTile> tiles = new List<FarmTile>();

        void Start()
        {
            Debug.Assert(farmTilePrefab, "FarmTileManager requires a farmTilePrefab");
            Debug.Assert(dayController, "FarmTileManager requires a dayController");

            ValidateGrid();

            if (GameManager.Instance.SavedFarmData != null)
                RestoreGridState(GameManager.Instance.SavedFarmData);
        }

        void OnEnable()
        {
            dayController.dayPassedEvent.AddListener(this.OnDayPassed);
        }

        void OnDisable()
        {
            dayController.dayPassedEvent.RemoveListener(this.OnDayPassed);
        }

        public void OnDayPassed()
        {
            // Soil decay only — NOT plant withering anymore
            foreach (FarmTile farmTile in tiles)
                farmTile.OnDayPassed();
        }

        // ---------------- SAVE ----------------

        public FarmGridData CaptureGridState()
        {
            FarmGridData gridData = new FarmGridData();

            foreach (var tile in tiles)
            {
                FarmTileData data = new FarmTileData();

                data.condition = tile.GetCondition;
                data.daysSinceLastInteraction = tile.GetDaysSinceLastInteraction();
                data.hasPlant = tile.HasGrowTime();

                if (data.hasPlant)
                {
                    // NEW: save growth stage
                    if (tile.IsPlantWithered()) data.growthStage = 3;
                    else if (tile.IsPlantGrown()) data.growthStage = 2;
                    else if (tile.IsPlantMedium()) data.growthStage = 1;
                    else data.growthStage = 0;
                }

                gridData.tiles.Add(data);
            }

            return gridData;
        }

        // ---------------- RESTORE ----------------

        public void RestoreGridState(FarmGridData data)
        {
            if (data == null || data.tiles.Count != tiles.Count)
                return;

            for (int i = 0; i < tiles.Count; i++)
            {
                FarmTile tile = tiles[i];
                FarmTileData saved = data.tiles[i];

                tile.ForceSetCondition(saved.condition);
                tile.SetDaysSinceLastInteraction(saved.daysSinceLastInteraction);

                if (saved.hasPlant)
                {
                    tile.ForceEnsureGrowTime();
                    GrowTime gt = tile.GetGrowTime();

                    // NEW: restore correct growth stage
                    gt.LoadState(saved.growthStage);
                }

                tile.ForceUpdateVisual();
            }
        }

        // ---------------- GRID CREATION ----------------

        void InstantiateTiles()
        {
            Vector3 spawnPos = transform.position;
            int count = 0;
            GameObject clone = null;

            for (int c = 0; c < cols; c++)
            {
                for (int r = 0; r < rows; r++)
                {
                    clone = Instantiate(farmTilePrefab, spawnPos, Quaternion.identity);
                    clone.name = "Farm Tile " + count++;
                    spawnPos.x += clone.transform.localScale.x + tileGap;
                    clone.transform.parent = transform;
                    tiles.Add(clone.GetComponent<FarmTile>());
                }
                spawnPos.z += clone.transform.localScale.z + tileGap;
                spawnPos.x = transform.position.x;
            }
        }

        void OnValidate()
        {
#if UNITY_EDITOR
            if (Application.isPlaying) return;

            EditorApplication.delayCall += () =>
            {
                if (this == null) return;
                ValidateGrid();
            };
#endif
        }

        void ValidateGrid()
        {
            if (!farmTilePrefab) return;

            tiles.Clear();
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent<FarmTile>(out var tile))
                    tiles.Add(tile);
            }

            int newCount = rows * cols;

            if (tiles.Count != newCount)
            {
                DestroyTiles();
                InstantiateTiles();
            }
        }

        void DestroyTiles()
        {
            foreach (FarmTile tile in tiles)
            {
#if UNITY_EDITOR
                DestroyImmediate(tile.gameObject);
#else
                Destroy(tile.gameObject);
#endif
            }
            tiles.Clear();
        }
    }
}