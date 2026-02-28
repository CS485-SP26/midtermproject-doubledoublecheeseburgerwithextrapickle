using UnityEngine;

public class Item
{
    public string itemName;
    public Sprite itemIcon;
    public int itemStackSize;
    public int maxStack;
    public bool isEmpty;

    public Item()
    {
        itemName = "None";
        itemIcon = null;
        itemStackSize = 0;
        maxStack = 99;
        isEmpty = true;
    }


    //public Item(string name, Sprite icon, int stackSize, int maxStack)
    //{
    //    this.itemName = name;
    //    this.itemIcon = icon;
    //    this.itemStackSize = stackSize;
    //    this.maxStack = maxStack;
    //}
}
