// FarmTile.cs
using System.Collections.Generic;
using UnityEngine;
using Environment;

namespace Farming 
{
    public class FarmTile : MonoBehaviour
    {
        public enum Condition { Grass, Tilled, Watered, Planted, Grown }

        [SerializeField] private Condition tileCondition = Condition.Grass; 

        [Header("Visuals")]
        [SerializeField] private Material grassMaterial;
        [SerializeField] private Material tilledMaterial;
        [SerializeField] private Material wateredMaterial;
        MeshRenderer tileRenderer;

        [Header("Audio")]
        [SerializeField] private AudioSource stepAudio;
        [SerializeField] private AudioSource tillAudio;
        [SerializeField] private AudioSource waterAudio;

        // NEW: growth reference (separate script)
        [Header("Growth")]
        [SerializeField] private GrowTime growTime; // assign in Inspector (can be on a child object)

        // NEW: prefab to spawn per tile (IMPORTANT)
        // Drag your TomatoStates prefab here (the parent that contains the 3 stages + GrowTime).
        [SerializeField] private GameObject tomatoStatesPrefab;

        // NEW: optional spawn offset (if your model needs to sit slightly above ground)
        [SerializeField] private Vector3 tomatoSpawnLocalOffset = Vector3.zero;

        List<Material> materials = new List<Material>();

        private int daysSinceLastInteraction = 0;
        public FarmTile.Condition GetCondition { get { return tileCondition; } } // TODO: Consider what the set would do?

        void Start()
        {
            tileRenderer = GetComponent<MeshRenderer>();

            foreach (Transform edge in transform)
            {
                // FIX: some children (like TomatoStates) may NOT have a MeshRenderer
                MeshRenderer mr = edge.gameObject.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    materials.Add(mr.material);
                }
            }

            UpdateVisual();
        }

        public void Interact()
        {
            switch(tileCondition)
            {
                case FarmTile.Condition.Grass: Till(); break;
                case FarmTile.Condition.Tilled: Water(); break;
                case FarmTile.Condition.Watered: Debug.Log("Ready for planting"); break;
                case FarmTile.Condition.Planted: Debug.Log("Growing..."); break;
                case FarmTile.Condition.Grown: Debug.Log("Fully grown!"); break;
            }
            daysSinceLastInteraction = 0;
        }

        public void Till()
        {
            // prevent tilling while plant is growing/grown
            if (tileCondition == Condition.Planted || tileCondition == Condition.Grown) return;

            tileCondition = FarmTile.Condition.Tilled;
            UpdateVisual();
            tillAudio?.Play();
        }

        public void Water()
        {
            // prevent watering while plant is growing/grown
            if (tileCondition == Condition.Planted || tileCondition == Condition.Grown) return;

            tileCondition = FarmTile.Condition.Watered;
            UpdateVisual();
            waterAudio?.Play();
        }

        // NEW: call this from Farmer when player uses 1 seed on a Watered tile
        public bool PlantSeed()
        {
            if (tileCondition != Condition.Watered)
            {
                Debug.Log("[FarmTile] Tried to plant seed but tile is not watered.");
                return false;
            }

            EnsureGrowTimeInstance();

            tileCondition = Condition.Planted;
            UpdateVisual();

            if (growTime != null)
            {
                // ✅ FIX: GrowTime must be active to start coroutines
                if (!growTime.gameObject.activeInHierarchy)
                {
                    growTime.gameObject.SetActive(true);
                }

                // Starts: 0 sec planted, 15 sec medium, 30 sec fully grown
                growTime.StartGrowth();
            }
            else
            {
                Debug.LogWarning("[FarmTile] GrowTime not assigned/found. Growth visuals won't update.");
            }

            return true;
        }

        // NEW: spawns TomatoStates prefab under this tile so each tile grows independently
        private void EnsureGrowTimeInstance()
        {
            // If we already have a GrowTime under THIS tile, we are good
            if (growTime != null && growTime.transform != null && growTime.transform.IsChildOf(transform))
            {
                // ✅ ensure it's active
                if (!growTime.gameObject.activeInHierarchy)
                    growTime.gameObject.SetActive(true);
                return;
            }

            // Try to find one in children first (including inactive)
            GrowTime found = GetComponentInChildren<GrowTime>(true);
            if (found != null && found.transform.IsChildOf(transform))
            {
                growTime = found;

                // ✅ ensure it's active
                if (!growTime.gameObject.activeInHierarchy)
                    growTime.gameObject.SetActive(true);

                return;
            }

            // If none exists, spawn a unique TomatoStates under THIS tile
            if (tomatoStatesPrefab == null)
            {
                Debug.LogWarning($"[FarmTile] No tomatoStatesPrefab assigned on {name}. Assign it in the Inspector.");
                return;
            }

            GameObject instance = Instantiate(tomatoStatesPrefab, transform);

            // ✅ FIX: make sure the spawned TomatoStates is active
            instance.SetActive(true);

            instance.name = tomatoStatesPrefab.name; // cleaner hierarchy (optional)
            instance.transform.localPosition = tomatoSpawnLocalOffset;
            instance.transform.localRotation = Quaternion.identity;

            // ✅ FIX: cancel out parent scale so plant isn't squished by tile scaling
            Vector3 p = transform.lossyScale;
            instance.transform.localScale = new Vector3(
                p.x != 0f ? 1f / p.x : 1f,
                p.y != 0f ? 1f / p.y : 1f,
                p.z != 0f ? 1f / p.z : 1f
            );

            // Grab GrowTime from the spawned instance (including inactive children)
            growTime = instance.GetComponentInChildren<GrowTime>(true);

            // ✅ ensure GrowTime object itself is active
            if (growTime != null && !growTime.gameObject.activeInHierarchy)
                growTime.gameObject.SetActive(true);
        }

        // NEW: optional helper if you want Farmer to check whether it's fully grown
        public bool IsFullyGrown()
        {
            return tileCondition == Condition.Grown || (growTime != null && growTime.IsGrown);
        }

        private void Update()
        {
            // NEW: keep tile condition synced once growth finishes
            if (tileCondition == Condition.Planted && growTime != null && growTime.IsGrown)
            {
                tileCondition = Condition.Grown;
                UpdateVisual();
            }
        }

        private void UpdateVisual()
        {
            if(tileRenderer == null) return;
            switch(tileCondition)
            {
                case FarmTile.Condition.Grass: tileRenderer.material = grassMaterial; break;
                case FarmTile.Condition.Tilled: tileRenderer.material = tilledMaterial; break;
                case FarmTile.Condition.Watered: tileRenderer.material = wateredMaterial; break;

                // NEW: keep ground looking watered while planted/grown (you can swap materials later if you want)
                case FarmTile.Condition.Planted: tileRenderer.material = wateredMaterial; break;
                case FarmTile.Condition.Grown: tileRenderer.material = wateredMaterial; break;
            }
        }

        public void SetHighlight(bool active)
        {
            foreach (Material m in materials)
            {
                if (active)
                {
                    m.EnableKeyword("_EMISSION");
                } 
                else 
                {
                    m.DisableKeyword("_EMISSION");
                }
            }
            if (active) stepAudio.Play();
        }

        public void OnDayPassed()
        {
            daysSinceLastInteraction++;

            // NEW: don't decay while planted/grown
            if (tileCondition == Condition.Planted || tileCondition == Condition.Grown) return;

            if(daysSinceLastInteraction >= 2)
            {
                if(tileCondition == FarmTile.Condition.Watered) tileCondition = FarmTile.Condition.Tilled;
                else if(tileCondition == FarmTile.Condition.Tilled) tileCondition = FarmTile.Condition.Grass;
            }
            UpdateVisual();
        }
    }
}