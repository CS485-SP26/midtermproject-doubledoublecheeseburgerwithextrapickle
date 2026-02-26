using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Core;

public class UIStateLoader : MonoBehaviour
{
    [SerializeField] TMP_Text fundsText;
    [SerializeField] Image waterFillImage;
    [SerializeField] Image energyFillImage;
    [SerializeField] TMP_Text seedsText;

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("[UIStateLoader] GameManager.Instance is null. Make sure a GameManager object exists in this scene (or was carried over with DontDestroyOnLoad).");
            enabled = false;
            return;
        }

        float water = GameManager.Instance.GetWaterLevel();
        int funds = GameManager.Instance.GetFunds();
        int seeds = GameManager.Instance.GetSeeds();
        float energy = GameManager.Instance.GetEnergyLevel();

        Debug.Log($"[UIStateLoader] Scene={UnityEngine.SceneManagement.SceneManager.GetActiveScene().name} " +
                  $"Funds={funds}, Water={water}, Energy={energy}");

        // One-time missing reference warnings (so you see what's not assigned)
        if (fundsText == null) Debug.LogError("[UIStateLoader] fundsText is not assigned in the Inspector.");
        if (seedsText == null) Debug.LogError("[UIStateLoader] seedsText is not assigned in the Inspector.");
        if (waterFillImage == null) Debug.LogError("[UIStateLoader] waterFillImage is not assigned in the Inspector.");
        if (energyFillImage == null) Debug.LogError("[UIStateLoader] energyFillImage is not assigned in the Inspector.");
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;

        // Only update what exists (prevents NullReferenceException)
        if (fundsText != null)
            fundsText.text = "Funds: " + GameManager.Instance.GetFunds();

        if (seedsText != null)
            seedsText.text = "Seeds: " + GameManager.Instance.GetSeeds();

        if (waterFillImage != null)
            waterFillImage.fillAmount = Mathf.Clamp01(GameManager.Instance.GetWaterLevel());

        if (energyFillImage != null)
            energyFillImage.fillAmount = Mathf.Clamp01(GameManager.Instance.GetEnergyLevel() / 100f);
    }
}