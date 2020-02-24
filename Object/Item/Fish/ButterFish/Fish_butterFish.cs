using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_butterFish : Item
{
    protected override void Init()
    {
        _itemCode = (int)ItemMaster.ItemList.Fish_BUTTERFISH;
    }
    public override void UseItem()
    {
        Debug.Log((ItemMaster.ItemList)_itemCode);
    }
}
