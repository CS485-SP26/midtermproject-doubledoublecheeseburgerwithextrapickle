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
        [SerializeField] private float waterLevel = 1f;
        [SerializeField] private float waterPerUse = 0.1f;
        AnimatedController animatedController;
        [SerializeField] private TMP_Text fundsText;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Debug.Assert(waterCan != null, "Water Can is not assigned in the inspector.");
            Debug.Assert(hoe != null, "Hoe is not assigned in the inspector.");
            Debug.Assert(waterLevelUI != null, "Water Level is not assigned in the inspector.");
           
            SetTool("None");
            animatedController = GetComponent<AnimatedController>();
            waterLevelUI.SetFill(waterLevel);

            fundsText.text = "Funds: $" + GameManager.Instance.getFunds();
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
                    if(waterLevel > waterPerUse)
                    {
                        tile.Interact();
                        animatedController.SetTrigger("Water");
                        waterLevel -= waterPerUse;
                        waterLevelUI.SetFill(waterLevel);
                    }
                        
                    break;
                default: break;
            }
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