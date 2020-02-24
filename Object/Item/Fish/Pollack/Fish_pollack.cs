using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_pollack : Item
{
    protected override void Init()
    {
        _itemCode = (int)ItemMaster.ItemList.FISH_POLLACK;
    }
    public override void UseItem()
    {
        Debug.Log((ItemMaster.ItemList)_itemCode);
    }
}
