using Core;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    
    public void SwapScene(string sceneName)
    {
        Debug.Log($"Swapping to scene: {sceneName}");
        // Implement scene swapping logic here, e.g., using UnityEngine.SceneManagement

        GameManager.Instance.LoadScenebyName(sceneName);

    }
}
