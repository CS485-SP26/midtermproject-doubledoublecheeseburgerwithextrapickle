using Core;
using UnityEngine;

public class PurchaseHandler : MonoBehaviour
{

    [SerializeField] private GameObject purchaseButton;
    public int seedCost = 2;

    private void Start()
    {
        if (purchaseButton != null)
        {
            purchaseButton.SetActive(false);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collided with purchase item.");
            purchaseButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited collision with purchase item.");
            purchaseButton.SetActive(false);
        }
    }

    public void PurchaseItem()
    {
        Debug.Log("Seeds: " + GameManager.Instance.GetSeeds());
        Debug.Log("PurchaseItem() called");
        if(!GameManager.Instance.CanAfford(seedCost))
        {
            Debug.Log("Not enough funds to purchase seeds.");
            return;
        }
        GameManager.Instance.AddSeeds(1);
        GameManager.Instance.SubtractFunds(seedCost);
    }
}

