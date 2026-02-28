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
        if (tool == null)
        {
            farmer.SetTool("None");
            return;
        }
        farmer.SetTool(tool.itemName);
    }


}
