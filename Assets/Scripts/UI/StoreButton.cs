using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class StoreButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private TextMeshProUGUI storeText;
    public void setText(string text) 
    {
        storeText.text = text;
    }
}
