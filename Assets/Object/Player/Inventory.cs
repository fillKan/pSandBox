using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemSlot[]   ItemSlots;
    public ItemSlot EquipItemSlot;

    public void AddItem(DroppedItem item)
    {
        var itemName = item.Name;
        int emptySlotIndex = -1;

        for (int i = 0; i < ItemSlots.Length; i++)
        {
            if (ItemSlots[i].ContainItem == itemName) 
            {
                ItemSlots[i].AddItem();
                ItemMaster.Instance.AddDroppedItem(item);
                return;
            }
            else if (emptySlotIndex == -1 && ItemSlots[i].ContainItem == default)
            {
                emptySlotIndex = i;
            }
        }
        if (emptySlotIndex != -1)
        {
            ItemSlots[emptySlotIndex].AddItem(itemName);
            ItemMaster.Instance.AddDroppedItem(item);
        }
    }
}
