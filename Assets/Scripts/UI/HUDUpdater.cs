using TMPro;
using UnityEngine;
using Core;

public class HUDUpdater : MonoBehaviour
{
    [Header("Bars (ProgressBar components)")]
    [SerializeField] private ProgressBar waterBar;   // expects 0..1
    [SerializeField] private ProgressBar energyBar;  // expects 0..1 (we convert 0..100 -> /100)

    [Header("Text")]
    [SerializeField] private TMP_Text fundsText;
    [SerializeField] private TMP_Text seedsText;

    private void Update()
    {
        if (GameManager.Instance == null) return;

        // Text
        if (fundsText != null)
            fundsText.text = "Funds: " + GameManager.Instance.GetFunds();

        if (seedsText != null)
            seedsText.text = "Seeds: " + GameManager.Instance.GetSeeds();

        // Bars
        if (waterBar != null)
            waterBar.SetFill(Mathf.Clamp01(GameManager.Instance.GetWaterLevel())); // 0..1

        if (energyBar != null)
            energyBar.SetFill(Mathf.Clamp01(GameManager.Instance.GetEnergyLevel() / 100f)); // 0..100 -> 0..1
    }
}