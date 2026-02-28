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
        public int selectedSlotIndex = 0;
        public event Action OnInventoryChanged;

        float energyLevel = 100f;
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
            Item seedStack = Inventory.Find(i => i.itemName == "Seed");

            if(seedStack != null)
            {
                seedStack.itemStackSize = Mathf.Clamp(seedStack.itemStackSize + amount, 0, seedStack.maxStack);
            }
            else
            {
                Inventory.Add(new Item { itemIcon = Resources.Load<Sprite>("Seed"), itemName = "Seed", itemStackSize = amount, maxStack = 99, isEmpty = false });
            }

           OnInventoryChanged?.Invoke();



        }
        public void SubtractSeeds(int amount)
        {
            seeds -= amount;
            if (seeds < 0) seeds = 0;

            Item seedStack = Inventory.Find(i => i.itemName == "Seed");
            if (seedStack != null)
            {
                seedStack.itemStackSize = Mathf.Clamp(seedStack.itemStackSize - amount, 0, seedStack.maxStack);

            }

            OnInventoryChanged?.Invoke();
        }

        public void AddTomato(int amount)
        {
            Item tomatoes = Inventory.Find(i => i.itemName == "Tomato");
            if(tomatoes != null)
            {
                tomatoes.itemStackSize = Mathf.Clamp(tomatoes.itemStackSize + amount, 0, tomatoes.maxStack);
            }
            else
            {
                Inventory.Add(new Item { itemIcon = Resources.Load<Sprite>("Tomato"), itemName = "Tomato", itemStackSize = 1, maxStack = 99, isEmpty = false });
            }
            OnInventoryChanged?.Invoke();
        }

        public void SubtractTomato(int amount)
        {
            Item tomatoes = Inventory.Find(i => i.itemName == "Tomato");
            if (tomatoes != null)
            {
                tomatoes.itemStackSize = Mathf.Clamp(tomatoes.itemStackSize - amount, 0, tomatoes.maxStack);
            }
            OnInventoryChanged?.Invoke();
        }

        public int GetTomatoCount()
        {
            Item tomatoes = Inventory.Find(i => i.itemName == "Tomato");
            return tomatoes != null ? tomatoes.itemStackSize : 0;
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

        public void SetEnergyLevel(float value)
        {
            energyLevel = value;
        }

        public float GetEnergyLevel()
        {
            return energyLevel;
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
