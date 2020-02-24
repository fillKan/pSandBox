using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_pufferFish : Item
{
    protected override void Init()
    {
        _itemCode = (int)ItemMaster.ItemList.FISH_PUFFERFISH;
    }
    public override void UseItem()
    {
        Debug.Log((ItemMaster.ItemList)_itemCode);
    }
}
