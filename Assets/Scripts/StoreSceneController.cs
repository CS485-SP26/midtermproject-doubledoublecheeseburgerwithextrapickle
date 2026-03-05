using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoreSceneController : MonoBehaviour
{
    public void ExitStore()
    {
        Debug.Log("ExitStore() called");
        GameManager.Instance.LoadScenebyName("Scene1-FarmingSim");
    }
}
