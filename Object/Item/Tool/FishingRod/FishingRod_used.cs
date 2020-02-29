using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod_used : Item, ItemFunction
{
    public IEnumerator CarryItem(ItemSlot itemSlot)
    {


        yield break;
    }

    public IEnumerator EquipItem()
    {
        yield break;
    }

    public IEnumerator InSlotItem()
    {
        yield break;
    }

    public IEnumerator UseItem<T>(T xValue) where T : Interaction
    {
        yield break;
    }

    protected override void Init()
    {
        _itemCode = (int)ItemMaster.ItemList.FISHING_ROD_USED;
    }
}
