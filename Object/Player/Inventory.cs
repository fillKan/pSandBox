using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemSlot[] itemSlots;

    public void AddItemInventory(Item item)
    {
        item.gameObject.SetActive(false);

        for(int i = 0; i < itemSlots.Length; i++)
        {
            if(itemSlots[i].ContainItem == null)
            {
                itemSlots[i].AddItem(item);
                break;
            }

            else if(itemSlots[i].ContainItem.itemCode == item.itemCode)
            {
                itemSlots[i].AddItem(item);
                break;
            }
        }
    }



}
