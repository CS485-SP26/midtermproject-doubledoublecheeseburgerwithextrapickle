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
        [SerializeField] private TMP_Text fundsText;
        [SerializeField] private GameObject WinText;
        
        //[SerializeField] private TMP_Text seedsText;        
        //private bool hasAwardedCompletion = false;
        private bool buyButtonClicked = false; // TODO: Connect this to UI button (I believe)

        private AnimatedController animatedController;

        void Start()
        {
            Debug.Assert(waterCan != null, "Water Can is not assigned in the inspector.");
            Debug.Assert(hoe != null, "Hoe is not assigned in the inspector.");
            Debug.Assert(waterLevelUI != null, "Water Level is not assigned in the inspector.");

            SetTool("None");
            animatedController = GetComponent<AnimatedController>();

            // 🔹 Read from GameManager, not a local serialized value
            float water = GameManager.Instance.GetWaterLevel();
            waterLevelUI.SetFill(water);

            WinText.SetActive(false);


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
                    tile.Interact();
                    animatedController.SetTrigger("Till");
                    break;

                case FarmTile.Condition.Tilled:
                    {
                        float before = GameManager.Instance.GetWaterLevel();

                        // 🔹 Don’t water if empty
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
            if(GameManager.Instance.hasAwardedCompletion) return;

            if(GameManager.Instance.GetSeeds() >= 5)
            {
                GameManager.Instance.setWinCondition();
                GameManager.Instance.AddFunds(30);
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
