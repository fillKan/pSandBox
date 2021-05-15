using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemSlot[] itemSlots;
    public ItemSlot CarryItemSlot;

    private sbyte empty = -1;

    public void AddItemInventory(DroppedItem item)
    {
        int emptySlotIndex = empty;

        for(int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].ContainItem == null)
            {
                if (emptySlotIndex.Equals(empty))
                {
                    emptySlotIndex = i;
                }
                continue;
            }

            if (itemSlots[i].ContainItem == item.ItemData)
            {
                itemSlots[i].AddItem(item.ItemData);
                ItemMaster.Instance.AddDroppedItem(item);
                return;
            }
        }
        if(!emptySlotIndex.Equals(empty))
        {
            itemSlots[emptySlotIndex].AddItem(item.ItemData);
            ItemMaster.Instance.AddDroppedItem(item);
            return;
        }
        Debug.LogWarning("인벤토리가 가득 차 있습니다");
    }



}
