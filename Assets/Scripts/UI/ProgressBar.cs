using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI fillText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float Fill {set { fillImage.fillAmount = value; } }

    public void SetFill(float value)
    {
        Fill = value;
        //fillImage.fillAmount = value;
    }

    public void setText(string text)
    {
        fillText.text = text;
    }

}
