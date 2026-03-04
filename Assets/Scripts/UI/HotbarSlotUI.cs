using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI countText;
    public Image border;
    public InventoryController inventoryController;
    public bool IsSelected;

    public void SetItem(Item item)
    {
        // UPDATED: itemIcon → data.itemIcon
        icon.sprite = item.data.itemIcon;
        icon.enabled = true;

        // UPDATED: maxStack → data.maxStack
        if (item.data.maxStack > 1)
        {
            countText.gameObject.SetActive(true);
            countText.text = item.itemStackSize > 1 ? item.itemStackSize.ToString() : "";
        }
        else
        {
            countText.gameObject.SetActive(false);
        }
    }

    public void Clear()
    {
        icon.sprite = null;
        icon.enabled = false;
        countText.text = "";
    }

    public void SetSelected()
    {
        Debug.Log("Selected");
        inventoryController.SelectSlot(this);
    }

    public void Select()
    {
        SetColor(Color.yellow);
        IsSelected = true;
    }

    public void Deselect()
    {
        SetColor(Color.white);
        IsSelected = false;
    }

    public void SetColor(Color newColor)
    {
        Debug.Log("Color changed");
        border.color = newColor;
    }
}