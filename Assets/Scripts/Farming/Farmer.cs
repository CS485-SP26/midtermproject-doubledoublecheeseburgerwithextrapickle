using Character;
using Core;
using Farming;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Farming
{
    [RequireComponent(typeof(AnimatedController))]
    public class Farmer : MonoBehaviour
    {
        [SerializeField] private GameObject waterCan;
        [SerializeField] private GameObject hoe;
        [SerializeField] private ProgressBar waterLevelUI;
        [SerializeField] private float waterPerUse = 0.1f;
        [SerializeField] private ProgressBar energyLevelUI;
        [SerializeField] private float energyPerUse = 15f;
        [SerializeField] private TMP_Text fundsText;
        [SerializeField] private GameObject WinText;
        
        //[SerializeField] private TMP_Text seedsText;        
        private bool hasAwardedCompletion = false;
        private bool buyButtonClicked = false; // TODO: Connect this to UI button (I believe)

        private AnimatedController animatedController;

        void Start()
{
    Debug.Assert(waterCan != null, "Water Can is not assigned in the inspector.");
    Debug.Assert(hoe != null, "Hoe is not assigned in the inspector.");
    Debug.Assert(waterLevelUI != null, "Water Level is not assigned in the inspector.");
    Debug.Assert(energyLevelUI != null, "Energy Level is not assigned in the inspector.");

    SetTool("None");
    animatedController = GetComponent<AnimatedController>();

   
    if (GameManager.Instance == null)
    {
        Debug.LogWarning("[Farmer] GameManager.Instance is null in this scene. Farmer will not initialize UI values.");
        return;
    }


    float water = GameManager.Instance.GetWaterLevel();   // 0..1
    waterLevelUI.SetFill(Mathf.Clamp01(water));

    float energy = GameManager.Instance.GetEnergyLevel(); // 0..100
    energyLevelUI.SetFill(Mathf.Clamp01(energy / 100f));

   
    if (WinText != null)
        WinText.SetActive(false);
    else
        Debug.LogWarning("[Farmer] WinText is not assigned in the inspector.");
}

        void Update()
        {
            winConditionMet(); 
        }
        
        public void TryTileInteraction(FarmTile tile)
        {
            if (tile == null) return;

            switch (tile.GetCondition)
            {
                case FarmTile.Condition.Grass:
                    {
                        float before = GameManager.Instance.GetEnergyLevel(); // 0..100

                        // Don't till if no energy
                        if (before < energyPerUse)
                        {
                            Debug.Log("[Farmer] Tried to perform but no energy left in the tank.");
                            return;
                        }

                        float after = Mathf.Clamp(before - energyPerUse, 0f, 100f);
                        GameManager.Instance.SetEnergyLevel(after);

                        Debug.Log($"[Farmer] Energy used. Before={before}, After={after}");

                        tile.Interact();
                        animatedController.SetTrigger("Till");

                        // ProgressBar expects 0..1
                        energyLevelUI.SetFill(after / 100f);
                    }
                    break;
                case FarmTile.Condition.Tilled:
                    {
                        float before = GameManager.Instance.GetWaterLevel();

                        // 🔹 Don’t water if no water
                        if (before <= 0f || before < waterPerUse)
                        {
                            Debug.Log("[Farmer] Tried to water but water is empty.");
                            return;
                        }

                        float after = Mathf.Clamp01(before - waterPerUse);
                        GameManager.Instance.SetWaterLevel(after);

                        Debug.Log($"[Farmer] Water used. Before={before}, After={after}");

                        tile.Interact();
                        animatedController.SetTrigger("Water");

                        // 🔹 Drive UI from GameManager value
                        waterLevelUI.SetFill(after);
                    }
                    break;
                case FarmTile.Condition.Watered:
                {
                    int seedsBefore = GameManager.Instance.GetSeeds();

                    if (seedsBefore < 1)
                    {
                        Debug.Log("[Farmer] Tried to plant seeds but have no seeds.");
                        return;
                    }

                    // Try to plant first 
                    bool planted = tile.PlantSeed();

                    if (!planted)
                    {
                        Debug.Log("[Farmer] PlantSeed failed.");
                        return;
                    }

                    GameManager.Instance.SubtractSeeds(1);

                    Debug.Log($"[Farmer] Seed used. Before={seedsBefore}, After={GameManager.Instance.GetSeeds()}");

                    

                    break;
                }
                case FarmTile.Condition.Withered:
                    {
                        // Don't till if no energy
                        if (!hasEnergy()){ return; }

                        float beforeEnergy = GameManager.Instance.GetEnergyLevel(); // 0..100
                        float afterEnergy = Mathf.Clamp(beforeEnergy - energyPerUse, 0f, 100f);
                        GameManager.Instance.SetEnergyLevel(afterEnergy);

                        Debug.Log($"[Farmer] Energy used. Before={beforeEnergy}, After={afterEnergy}");

                        tile.Interact();
                        animatedController.SetTrigger("Till");

                        // ProgressBar expects 0..1
                        energyLevelUI.SetFill(afterEnergy / 100f);
                    }
                    break;
                    
                default:
                    break;
            }
        }
        //public void CheckAllTilesWatered()
        //{
        //        if (hasAwardedCompletion) return;
                
        //        FarmTile[] allTiles = FindObjectsOfType<FarmTile>();
        //        foreach (FarmTile tile in allTiles)
        //        {
        //            if (tile.GetCondition != FarmTile.Condition.Watered)
        //            {
        //                return; // If any tile is not watered
        //            }
        //        }
                
        //        hasAwardedCompletion = true;
        //        GameManager.Instance.AddFunds(30); // Add funds for watering all tiles
        //        fundsText.text = "Congratulations! You earned money money money!!!\nFunds: $" + GameManager.Instance.GetFunds();
                
        //}

        public void winConditionMet()
        {
            if (hasAwardedCompletion) return;

            if (GameManager.Instance == null)
                return; // GameManager not ready in this scene

            if (GameManager.Instance.GetSeeds() >= 5)
            {
                hasAwardedCompletion = true;
                GameManager.Instance.AddFunds(30);

                if (WinText != null)
                    WinText.SetActive(true);
                else
                    Debug.LogWarning("[Farmer] WinText is not assigned in the inspector.");
            }
        }

        public void OnBuyButtonClicked()
        {
            buyButtonClicked = true;
        }

        public bool hasWater()
        {
            float beforeWater = GameManager.Instance.GetWaterLevel();
            if (beforeWater <= 0f || beforeWater < waterPerUse)
            {
                Debug.Log("[Farmer] Tried to water but water is empty.");
                return false;
            }
            return true;
        }
        public bool hasEnergy()
        {
            float beforeEnergy = GameManager.Instance.GetEnergyLevel(); // 0..100
            // Don't till if no energy
            if (beforeEnergy < energyPerUse)
            {
                Debug.Log("[Farmer] Tried to perform but no energy left in the tank.");
                return false;
            }
            return true;
        }

        //public void buySeeds()
        //{
        //    if (GameManager.Instance.GetFunds() >= 10 && buyButtonClicked)
        //    {
        //        GameManager.Instance.SubtractFunds(10);
        //        GameManager.Instance.AddSeeds(1);
        //        fundsText.text = "Item bought!\nFunds: $" + GameManager.Instance.GetFunds();
        //        seedsText.text = "Seeds: " + GameManager.Instance.GetSeeds();
        //        buyButtonClicked = false; 
        //    }
        //}
        
        public void SetTool(string tool)
        {
            waterCan.SetActive(false);
            hoe.SetActive(false);

            switch (tool)
            {
                case "Watering Can":
                    waterCan.SetActive(true);
                    break;
                case "Hoe":
                    hoe.SetActive(true);
                    break;
                case "None":
                    waterCan.SetActive(false);
                    hoe.SetActive(false);
                    break;
            }
        }
    }
}
