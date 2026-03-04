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

        private bool buyButtonClicked = false;

        private AnimatedController animatedController;
        private InventoryController inventoryController;

        void Start()
        {
            Debug.Assert(waterCan != null, "Water Can is not assigned in the inspector.");
            Debug.Assert(hoe != null, "Hoe is not assigned in the inspector.");
            Debug.Assert(waterLevelUI != null, "Water Level is not assigned in the inspector.");
            Debug.Assert(energyLevelUI != null, "Energy Level is not assigned in the inspector.");

            SetTool("None");
            animatedController = GetComponent<AnimatedController>();
            inventoryController = GetComponent<InventoryController>();

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
                        // UPDATED: itemName → data.itemName
                        if (inventoryController.GetSelectedItem().data.itemName != "Hoe")
                            break;

                        float before = GameManager.Instance.GetEnergyLevel();
                        if (before < energyPerUse)
                        {
                            Debug.Log("[Farmer] Tried to perform but no energy left.");
                            return;
                        }

                        float after = Mathf.Clamp(before - energyPerUse, 0f, 100f);
                        GameManager.Instance.SetEnergyLevel(after);
                        energyLevelUI.SetFill(after / 100f);

                        tile.Interact();
                        animatedController.SetTrigger("Till");
                        break;
                    }

                case FarmTile.Condition.Tilled:
                    {
                        // UPDATED
                        if (inventoryController.GetSelectedItem().data.itemName != "Watering Can")
                            break;

                        float before = GameManager.Instance.GetWaterLevel();
                        if (before <= 0f || before < waterPerUse)
                        {
                            Debug.Log("[Farmer] Tried to water but water is empty.");
                            return;
                        }

                        float after = Mathf.Clamp01(before - waterPerUse);
                        GameManager.Instance.SetWaterLevel(after);
                        waterLevelUI.SetFill(after);

                        tile.Interact();
                        animatedController.SetTrigger("Water");
                        break;
                    }

                case FarmTile.Condition.Withered:
                    {
                        // UPDATED
                        if (inventoryController.GetSelectedItem().data.itemName != "Hoe")
                            break;

                        if (!hasEnergy()) return;

                        float before = GameManager.Instance.GetEnergyLevel();
                        float after = Mathf.Clamp(before - energyPerUse, 0f, 100f);
                        GameManager.Instance.SetEnergyLevel(after);
                        energyLevelUI.SetFill(after / 100f);

                        tile.Interact();
                        animatedController.SetTrigger("Till");
                        break;
                    }

                case FarmTile.Condition.Grown:
                    {
                        // UPDATED
                        if (inventoryController.GetSelectedItem().data.itemName != "Hoe")
                            break;

                        if (!hasEnergy()) return;

                        float before = GameManager.Instance.GetEnergyLevel();
                        float after = Mathf.Clamp(before - energyPerUse, 0f, 100f);
                        GameManager.Instance.SetEnergyLevel(after);
                        energyLevelUI.SetFill(after / 100f);

                        tile.Interact();
                        animatedController.SetTrigger("Till");
                        GameManager.Instance.AddTomato(1);
                        break;
                    }

                case FarmTile.Condition.Watered:
                    {
                        int seedsBefore = GameManager.Instance.GetSeeds();
                        if (seedsBefore < 1)
                        {
                            Debug.Log("[Farmer] Tried to plant seeds but have none.");
                            return;
                        }

                        if (!tile.PlantSeed())
                        {
                            Debug.Log("[Farmer] PlantSeed failed.");
                            return;
                        }

                        GameManager.Instance.SubtractSeeds(1);
                        break;
                    }

                default:
                    break;
            }
        }

        public void winConditionMet()
        {
            if (GameManager.Instance == null)
                return;

            if (GameManager.Instance.hasAwardedCompletion)
                return;

            if (GameManager.Instance.GetSeeds() >= 5)
            {
                GameManager.Instance.setWinCondition();
                GameManager.Instance.AddFunds(30);

                if (WinText != null)
                    WinText.SetActive(true);
            }
        }

        public void clearWin()
        {
            WinText.SetActive(false);
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
            float beforeEnergy = GameManager.Instance.GetEnergyLevel();
            if (beforeEnergy < energyPerUse)
            {
                Debug.Log("[Farmer] Tried to perform but no energy left in the tank.");
                return false;
            }
            return true;
        }

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