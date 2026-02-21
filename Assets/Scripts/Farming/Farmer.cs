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

            fundsText.text = "Funds: $" + GameManager.Instance.GetFunds();
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