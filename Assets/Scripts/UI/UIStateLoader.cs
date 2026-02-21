using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Core;

public class UIStateLoader : MonoBehaviour
{
    [SerializeField] TMP_Text fundsText;
    [SerializeField] Image waterFillImage;

    private void Start()
    {
        float water = GameManager.Instance.GetWaterLevel();
        int funds = GameManager.Instance.GetFunds();

        Debug.Log($"[UIStateLoader] Scene={UnityEngine.SceneManagement.SceneManager.GetActiveScene().name} " +
                  $"Funds={funds}, Water={water}");

        fundsText.text = "Funds: " + funds;
        waterFillImage.fillAmount = water;
    }
}