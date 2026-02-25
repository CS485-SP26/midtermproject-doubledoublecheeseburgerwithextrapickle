using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI countText;
    public Image border;
    public InventoryController inventoryController;

    public void SetItem(Item item)
    {
        icon.sprite = item.itemIcon;
        icon.enabled = true;

        if (item.maxStack > 1)
            countText.text = item.maxStack.ToString();
        else
            countText.text = "";
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
        SetColor(Color.yellow);
        inventoryController.SelectSlot(this);

    }
    public void SetColor(Color newColor)
    {
        Debug.Log("Color changed");
        border.color = newColor;
    }



}
