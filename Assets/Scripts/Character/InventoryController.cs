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
        inventoryItems = new List<Item>();

        clearInventory();

        inventoryItems[0] = (new Item {  itemIcon = Resources.Load<Sprite>("Hoe"), itemName = "Hoe", itemStackSize = 1, maxStack = 1, isEmpty = false});
        inventoryItems[1] = (new Item { itemIcon = Resources.Load<Sprite>("WaterCan"), itemName = "Watering Can", itemStackSize = 1, maxStack = 1, isEmpty = false });
        equipmentManager = GetComponent<EquipmentManager>();

        RefreshHotbar();

        SelectSlot(slots[0]);
    }

    private void clearInventory()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            inventoryItems.Add(new Item
            {
                itemIcon = Resources.Load<Sprite>("Hotbar"),
                itemName = "None",
                itemStackSize = 0,
                maxStack = 0,
                isEmpty = true
            });

        }
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
            HotbarSlotUI slot = slots[i];
            slot.Deselect();
        }
        selectedSlot.Select();
        equipmentManager.EquipItemToHand(GetSelectedItem());

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

    public string GetSelectedItemName(Item item)
    {
        return item != null ? item.itemName : "None";
    }

}
