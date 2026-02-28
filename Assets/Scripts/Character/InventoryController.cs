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
        inventoryItems = new List<Item>(GameManager.Instance.Inventory);
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

       
        inventoryItems.Add(new Item
        {
            itemIcon = Resources.Load<Sprite>("Hoe"),
            itemName = "Hoe",
            itemStackSize = 1,
            maxStack = 1,
            isEmpty = false
        });

       
        inventoryItems.Add(new Item
        {
            itemIcon = Resources.Load<Sprite>("WaterCan"),
            itemName = "Watering Can",
            itemStackSize = 1,
            maxStack = 1,
            isEmpty = false
        });


       
    }




    //private void clearInventory()
    //{
    //    for(int i = 0; i < slots.Length; i++)
    //    {
    //        inventoryItems.Add(new Item
    //        {
    //            itemIcon = Resources.Load<Sprite>("Hotbar"),
    //            itemName = "None",
    //            itemStackSize = 0,
    //            maxStack = 0,
    //            isEmpty = true
    //        });

    //    }
    //}

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
        equipmentManager.EquipItemToHand(GetSelectedItem());

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
                return inventoryItems[i];
        }
        return null;
    }

    //public string GetSelectedItemName(Item item)
    //{
    //    return item != null ? item.itemName : "None";
    //}

    public void SaveToGameManager()
    {
        GameManager.Instance.Inventory = new List<Item>(inventoryItems);
    }

    
}
