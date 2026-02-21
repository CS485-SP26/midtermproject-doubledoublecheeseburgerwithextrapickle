using UnityEngine;
using Core;

public class TitleSceneLoader : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.AddFunds(10);
        GameManager.Instance.SetWaterLevel(1f);

        GameManager.Instance.LoadScenebyName("Scene1-FarmingSim");
    }
}