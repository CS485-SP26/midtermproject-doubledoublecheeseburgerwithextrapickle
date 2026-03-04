using Core;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public List<Item> inventoryItems;
    public HotbarSlotUI[] slots;
    EquipmentManager equipmentManager;

    public ItemData hoeData;
    public ItemData wateringCanData;
    public ItemData seedData;
    public ItemData tomatoData;


    public void Start()
    {

        GameManager.Instance.OnInventoryChanged += HandleInventoryChanged;
        equipmentManager = GetComponent<EquipmentManager>();
        if (GameManager.Instance.Inventory.Count == 0)
        {
            inventoryItems = new List<Item>();
            InitializeDefaultInventory();
            SaveToGameManager();
        }
        else
        {
            inventoryItems = new List<Item>(GameManager.Instance.Inventory);
        }


        RefreshHotbar();

        int index = Mathf.Clamp(GameManager.Instance.selectedSlotIndex, 0, slots.Length - 1);
        SelectSlot(slots[index]);
    }

    private void HandleInventoryChanged()
    {
        inventoryItems.Clear();

        // Always add starting tools
        inventoryItems.Add(CreateItem(hoeData));
        inventoryItems.Add(CreateItem(wateringCanData));

        // Add seeds if player has any
        int seedCount = GameManager.Instance.GetSeeds();
        if (seedCount > 0)
            inventoryItems.Add(CreateItem(seedData, seedCount));

        // Add tomatoes if player has any
        int tomatoCount = GameManager.Instance.GetTomatoCount();
        if (tomatoCount > 0)
            inventoryItems.Add(CreateItem(tomatoData, tomatoCount));

        SaveToGameManager();
        RefreshHotbar();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnInventoryChanged -= HandleInventoryChanged;
    }



    private void InitializeDefaultInventory()
    {
        inventoryItems = new List<Item>();

        AddItem(CreateItem(hoeData));
        AddItem(CreateItem(wateringCanData));


    }


    public void RefreshHotbar()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventoryItems.Count)
                slots[i].SetItem(inventoryItems[i]);
            else
                slots[i].Clear();
        }

    }

    public void SelectSlot(HotbarSlotUI selectedSlot)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            //HotbarSlotUI slot = slots[i];
            slots[i].Deselect();
        }
        selectedSlot.Select();
        GameManager.Instance.selectedSlotIndex = GetSlotIndex(selectedSlot);
        Item item = GetSelectedItem();

        if (item == null)
        {
            //equipmentManager.UnequipHand();   // or do nothing
            return;
        }

        equipmentManager.EquipItemToHand(item);

    }

    private int GetSlotIndex(HotbarSlotUI slot)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == slot)
                return i;
        }
        return 0;
    }

    public Item GetSelectedItem()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsSelected)
            {
                if (i < 0 || i >= inventoryItems.Count)
                    return null;

                return inventoryItems[i];
            }
        }

        return null;
    }


    public void SaveToGameManager()
    {
        GameManager.Instance.Inventory = new List<Item>(inventoryItems);
    }

    public void AddItem(Item newItem)
    {
        inventoryItems.Add(newItem);
        SaveToGameManager();
        RefreshHotbar();
    }


    public Item CreateItem(ItemData data, int amount = 1)
    {
        return new Item
        {
            data = data,
            itemStackSize = amount,
            isEmpty = false
        };
    }


}
