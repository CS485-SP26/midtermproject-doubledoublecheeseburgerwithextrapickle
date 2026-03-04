using Farming;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    Farmer farmer;

    void Start()
    {
        farmer = GetComponent<Farmer>();
        Debug.Assert(farmer, "EquipmentManager requires a Farmer component.");
    }

    public void EquipItemToHand(Item tool)
    {
        if (tool == null || tool.data == null)   // safer null check
        {
            farmer.SetTool("None");
            return;
        }

        // UPDATED: itemName → data.itemName
        farmer.SetTool(tool.data.itemName);
    }
}