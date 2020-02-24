using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBirch_seed : Item
{
    protected override void Init()
    {
        _itemCode = (int)ItemMaster.ItemList.SEED_WHITEBIRCH;
    }
    public override void UseItem()
    {
        Debug.Log((ItemMaster.ItemList)_itemCode);
    }
}