using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Core;

public class UIStateLoader : MonoBehaviour
{
    [SerializeField] TMP_Text fundsText;
    [SerializeField] Image waterFillImage;
    [SerializeField] TMP_Text seedsText;

    private void Start()
    {
        float water = GameManager.Instance.GetWaterLevel();
        int funds = GameManager.Instance.GetFunds();
        int seeds = GameManager.Instance.GetSeeds();

        Debug.Log($"[UIStateLoader] Scene={UnityEngine.SceneManagement.SceneManager.GetActiveScene().name} " +
                  $"Funds={funds}, Water={water}");

     
    }

    private void Update()
    {
        fundsText.text = "Funds: " + GameManager.Instance.GetFunds();
        seedsText.text = "Seeds: " + GameManager.Instance.GetSeeds();
        waterFillImage.fillAmount = GameManager.Instance.GetWaterLevel();
    }

}