using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : Item, ItemFunction
{
    public IEnumerator CarryItem()
    {
        if (!StartFunction(ref _isCarryItem)) yield break;

        // TODO . . .

        OverFunction(ref _isCarryItem);
        yield break;
    }

    public IEnumerator EquipItem()
    {
        if (!StartFunction(ref _isEquipItem)) yield break;

        // TODO . . .

        OverFunction(ref _isEquipItem);
        yield break;
    }

    public IEnumerator InSlotItem()
    {
        if (!StartFunction(ref _isInSlotItem)) yield break;

        // TODO . . .

        OverFunction(ref _isInSlotItem);
        yield break;
    }

    public IEnumerator UseItem<T>(T xValue) where T : Interaction
    {
        if (!StartFunction(ref _isUseItem)) yield break;

        // TODO . . .

        OverFunction(ref _isUseItem);
        yield break;
    }

    protected override void Init()
    {
        _itemCode = (int)ItemMaster.ItemList.FISHING_ROD;
    }
}
