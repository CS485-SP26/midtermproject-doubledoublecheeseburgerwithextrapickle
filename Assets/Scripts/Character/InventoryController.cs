using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public List<Item> inventoryItems;
    public HotbarSlotUI[] slots;

    public void Start()
    {
        inventoryItems = new List<Item>();

        inventoryItems.Add(new Item {  itemIcon = Resources.Load<Sprite>("Hoe"), itemName = "Hoe", itemStackSize = 1, maxStack = 1 });
        inventoryItems.Add(new Item { itemIcon = Resources.Load<Sprite>("WaterCan"), itemName = "Watering Can", itemStackSize = 1, maxStack = 1 });

        RefreshHotbar();
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
            slot.SetColor(Color.white);
        }
        selectedSlot.SetColor(Color.yellow);
    }



}
