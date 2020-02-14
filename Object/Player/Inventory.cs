using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemSlot[] itemSlots;
    private sbyte empty = -1;

    public void AddItemInventory(Item item)
    {
        int emptySlotIndex = empty;

        item.gameObject.SetActive(false);

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

            if(itemSlots[i].ContainItem.itemCode == item.itemCode)
            {
                itemSlots[i].AddItem(item);
                return;
            }
        }
        if(!emptySlotIndex.Equals(empty))
        {
            itemSlots[emptySlotIndex].AddItem(item);
            return;
        }
        Debug.LogError("인벤토리가 가득 차 있습니다");
    }



}
