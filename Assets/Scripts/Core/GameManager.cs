using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameManager:MonoBehaviour
    {
        private static GameManager instance = null;
        int funds = 0;
        

        static public GameManager Instance
        {
            get
            {
                if(instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    instance = go.AddComponent<GameManager>();
                    DontDestroyOnLoad(go);
                    Debug.Log("GameManager instance created");
                }
                return instance;
            }
        }

        public void Awake()
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
                Debug.Log("GameManager instance assigned in Awake");
            }
            else if(instance != this)
            {
                Destroy(this);
                Debug.LogWarning("Duplicate GameManager instance destroyed");
            }
        }

        public void AddFunds(int funds)
        {
            this.funds = funds;
        }

        public void LoadScenebyName(string name)
        {
            SceneManager.LoadScene(name);
        }

        public int getFunds()
        {
            return this.funds;
        }
    }
}