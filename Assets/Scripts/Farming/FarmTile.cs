// FarmTile.cs
using System.Collections.Generic;
using UnityEngine;
using Environment;

namespace Farming
{
    public class FarmTile : MonoBehaviour
    {
        public enum Condition { Grass, Tilled, Watered, Planted, Grown, Withered }

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
            switch (tileCondition)
            {
                case FarmTile.Condition.Grass: Till(); break;
                case FarmTile.Condition.Tilled: Water(); break;
                case FarmTile.Condition.Watered: Debug.Log("Ready for planting"); break;
                case FarmTile.Condition.Planted: Debug.Log("Growing..."); break;
                case FarmTile.Condition.Grown: Harvest(); Debug.Log("Fully grown!"); break;
                case FarmTile.Condition.Withered: Till(); break; // Hoping this works, and allows player to till and water normally again. if not im sorry.

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
            
            if (tileCondition == Condition.Planted || tileCondition == Condition.Grown) return;

            tileCondition = FarmTile.Condition.Watered;
            UpdateVisual();
            waterAudio?.Play();
        }

        public void Harvest()
        {
            if(tileCondition != Condition.Grown)
            {
                Debug.Log("[FarmTile] Tried to harvest but tile is not fully grown.");
                return;
            }
            tileCondition = FarmTile.Condition.Tilled;
            Debug.Log("[FarmTile] Harvested plant, resetting to tilled state.");
            growTime.RemovePlant();
            UpdateVisual();
            tillAudio?.Play();
        }

        
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
                
                if (!growTime.gameObject.activeInHierarchy)
                {
                    growTime.gameObject.SetActive(true);
                }

               
                growTime.StartGrowth();
            }
            else
            {
                Debug.LogWarning("[FarmTile] GrowTime not assigned/found. Growth visuals won't update.");
            }

            return true;
        }

        
        public bool IsFullyGrown()
        {
            return tileCondition == Condition.Grown || (growTime != null && growTime.IsGrown);
        }

       
        private void EnsureGrowTimeInstance()
        {
            
            if (growTime != null && growTime.transform != null && growTime.transform.IsChildOf(transform))
            {
                
                if (!growTime.gameObject.activeInHierarchy)
                    growTime.gameObject.SetActive(true);
                return;
            }

            
            GrowTime found = GetComponentInChildren<GrowTime>(true);
            if (found != null && found.transform.IsChildOf(transform))
            {
                growTime = found;

               
                if (!growTime.gameObject.activeInHierarchy)
                    growTime.gameObject.SetActive(true);

                return;
            }

           
            if (tomatoStatesPrefab == null)
            {
                Debug.LogWarning($"[FarmTile] No tomatoStatesPrefab assigned on {name}. Assign it in the Inspector.");
                return;
            }

            GameObject instance = Instantiate(tomatoStatesPrefab, transform);

            
            instance.SetActive(true);

            instance.name = tomatoStatesPrefab.name; 
            instance.transform.localPosition = tomatoSpawnLocalOffset;
            instance.transform.localRotation = Quaternion.identity;

           
            Vector3 p = transform.lossyScale;
            instance.transform.localScale = new Vector3(
                p.x != 0f ? 1f / p.x : 1f,
                p.y != 0f ? 1f / p.y : 1f,
                p.z != 0f ? 1f / p.z : 1f
            );

            
            growTime = instance.GetComponentInChildren<GrowTime>(true);

            
            if (growTime != null && !growTime.gameObject.activeInHierarchy)
                growTime.gameObject.SetActive(true);
        }

        private void Update()
        {
            
            if (tileCondition == Condition.Planted && growTime != null && growTime.IsGrown)
            {
                tileCondition = Condition.Grown;
                UpdateVisual();
            }
        }

        private void UpdateVisual()
        {
            if (tileRenderer == null) return;
            switch (tileCondition)
            {
                case FarmTile.Condition.Grass: tileRenderer.material = grassMaterial; break;
                case FarmTile.Condition.Tilled: tileRenderer.material = tilledMaterial; break;
                case FarmTile.Condition.Watered: tileRenderer.material = wateredMaterial; break;

                // NEW: keep ground looking watered while planted/grown (you can swap materials later if you want)
                case FarmTile.Condition.Planted: tileRenderer.material = wateredMaterial; break;
                case FarmTile.Condition.Grown: tileRenderer.material = wateredMaterial; break;
                case FarmTile.Condition.Withered: tileRenderer.material = tilledMaterial; break; // TODO: Might want to change to a withered material of the sorts
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
            // if (tileCondition == Condition.Planted || tileCondition == Condition.Grown) return;

            if (daysSinceLastInteraction >= 2)
            {
                if (tileCondition == FarmTile.Condition.Watered) tileCondition = FarmTile.Condition.Tilled;
                else if (tileCondition == FarmTile.Condition.Tilled) tileCondition = FarmTile.Condition.Grass;
                else if (tileCondition == Condition.Planted || tileCondition == FarmTile.Condition.Grown)
                {
                    tileCondition = FarmTile.Condition.Withered;

                    if (growTime != null)
                        growTime.SetWithered();
                }
            }
            UpdateVisual();
        }
    }
}