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

            if(itemSlots[i].ContainItem.ItemData == item.ItemData)
            {
                item.gameObject.SetActive(false);

                itemSlots[i].AddItem(item.ItemData);
                ItemMaster.Instance.StoreItemExisting(item);
                return;
            }
        }
        if(!emptySlotIndex.Equals(empty))
        {
            item.gameObject.SetActive(false);

            itemSlots[emptySlotIndex].AddItem(item.ItemData);
            ItemMaster.Instance.StoreItemExisting(item);
            return;
        }
        Debug.LogWarning("인벤토리가 가득 차 있습니다");
    }



}
