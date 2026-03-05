using Core;
using Farming;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopTrigger : SceneTrigger
{

    [SerializeField] private GameObject storeButtonObject;
    [SerializeField] private StoreButton storeButtonObjectScript;

    private void Start()
    {
        if (storeButtonObject != null)
        {
            storeButtonObject.SetActive(false);
        }


    }



    private void OnTriggerEnter(Collider other)
    {
        // TryGetComponent is faster than GetComponent if the component is uncertain
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger area.");
            storeButtonObjectScript.setText(setStoreText());
            storeButtonObject.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        // TryGetComponent is faster than GetComponent if the component is uncertain
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left the trigger area.");
            storeButtonObject.SetActive(false);

        }
    }

    public void EnterStore()
    {
        GameManager.Instance.LoadScenebyName("Scene2-Store");
    }

    public void ExitStore()
    {
        Debug.Log("ExitStore() called");

        GameManager.Instance.LoadScenebyName("Scene1-FarmingSim");
    }

    private string setStoreText()
    {
        return "Go To Store";
    }


}
