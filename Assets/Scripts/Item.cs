using UnityEngine;

[System.Serializable]
public class Item
{
    public ItemData data;       // reference to ScriptableObject
    public int itemStackSize;   // runtime value
    public bool isEmpty;

    public string Name => data != null ? data.itemName : "None";
    public Sprite Icon => data != null ? data.itemIcon : null;
    public int MaxStack => data != null ? data.maxStack : 0;

    public Item()
    {
        data = null;
        itemStackSize = 0;
        isEmpty = true;
    }
}