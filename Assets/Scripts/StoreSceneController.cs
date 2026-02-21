using UnityEngine;
using UnityEngine.SceneManagement;

public class StoreSceneController : MonoBehaviour
{
    public void ExitStore()
    {
        Debug.Log("ExitStore() called");
        SceneManager.LoadScene("Scene1-FarmingSim");
    }
}
