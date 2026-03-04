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
        int tomatoes = 0;

        float waterLevel = 1f;
        float energyLevel = 100f;

        public bool hasAwardedCompletion = false;

        public List<Item> Inventory = new List<Item>();
        public int selectedSlotIndex = 0;

        public event Action OnInventoryChanged;

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
            funds = Mathf.Max(0, funds - amount);
        }

        public int GetFunds() => funds;


        public void AddSeeds(int amount)
        {
            seeds += amount;
            OnInventoryChanged?.Invoke();
        }

        public void SubtractSeeds(int amount)
        {
            seeds = Mathf.Max(0, seeds - amount);
            OnInventoryChanged?.Invoke();
        }

        public int GetSeeds() => seeds;


        public void AddTomato(int amount)
        {
            tomatoes += amount;
            OnInventoryChanged?.Invoke();
        }

        public void SubtractTomato(int amount)
        {
            tomatoes = Mathf.Max(0, tomatoes - amount);
            OnInventoryChanged?.Invoke();
        }

        public int GetTomatoCount() => tomatoes;


        public void SetWaterLevel(float value)
        {
            waterLevel = value;
        }

        public float GetWaterLevel() => waterLevel;

        public void SetEnergyLevel(float value)
        {
            energyLevel = value;
        }

        public float GetEnergyLevel() => energyLevel;


        public void LoadScenebyName(string name)
        {
            SceneManager.LoadScene(name);
        }

        public bool CanAfford(int amount) => funds >= amount;

        public void setWinCondition()
        {
            hasAwardedCompletion = true;
        }
    }
}