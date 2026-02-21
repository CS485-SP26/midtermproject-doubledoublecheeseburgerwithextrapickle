using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        int funds = 0;
        float waterLevel = 1f; // will be overridden at game start

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void AddFunds(int amount)
        {
            funds += amount;
        }

        public int GetFunds()
        {
            return funds;
        }

        public void SetWaterLevel(float value)
        {
            waterLevel = value;
        }

        public float GetWaterLevel()
        {
            return waterLevel;
        }

        public void LoadScenebyName(string name)
        {
            SceneManager.LoadScene(name);
        }
    }
}