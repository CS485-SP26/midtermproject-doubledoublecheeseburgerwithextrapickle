using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        int funds = 0;
        int seeds = 0;
        float waterLevel = 1f;
        public bool hasAwardedCompletion = false;
        public List<Item> Inventory = new List<Item>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }


        public void AddFunds(int amount)
        {
            funds += amount;
        }

        public void SubtractFunds(int amount)
        {
            funds -= amount;
            if (funds < 0)
                funds = 0;
        }

        public void AddSeeds(int amount)
        {
            seeds += amount;
        }
        
        public int GetSeeds()
        {
            return seeds;
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

        public bool CanAfford(int amount)
        {
            return funds >= amount;
        }

        public void setWinCondition()
        {
            hasAwardedCompletion = true;
        }
    }
}
