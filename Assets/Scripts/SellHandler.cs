using Core;
using UnityEngine;

public class SellHandler : MonoBehaviour
{
    [SerializeField] private GameObject sellButton;
    public int plantSellPrice = 5;
    
    private void Start()
    {
        if (sellButton != null)        {
            sellButton.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collided with sell item.");
            sellButton.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited collision with sell item.");
            sellButton.SetActive(false);
        }
    }
    public void SellPlant()
    {
        Debug.Log("Tomatos: " + GameManager.Instance.GetTomatoCount());
        Debug.Log("SellPlant() called");
        if (GameManager.Instance.GetTomatoCount() <= 0)
        {
            Debug.Log("No tomatoes harvested to sell. :("); return;
        }
  
        GameManager.Instance.setSoldTomatoesCelebration();
        GameManager.Instance.SubtractTomato(1);
        GameManager.Instance.AddFunds(plantSellPrice); 
    
    }
}
