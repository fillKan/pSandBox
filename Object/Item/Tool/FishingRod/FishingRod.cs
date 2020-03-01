﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : Item, ItemFunction
{
    [Tooltip("낚시찌 오브젝트")]
    public Bobber bobber;

    public IEnumerator CarryItem(ItemSlot itemSlot)
    {
        if (!StartWorking(ref _isCarryItem)) yield break;

        if (MouseCursor.Instance.ClickVoid)
        {
            itemSlot.SetItem(ItemMaster.ItemList.FISHING_ROD_USED);
        }

        StopWorking(ref _isCarryItem);
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
        _itemCode = (int)ItemMaster.ItemList.FISHING_ROD;

        _itemType = ItemMaster.ItemType.TOOL;
    }
}
