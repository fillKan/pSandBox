using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBirch_log : Item
{
    protected override void Init()
    {
        _itemCode = (int)ItemMaster.ItemList.LOG_WHITEBIRCH;
    }
    public override void UseItem()
    {
        Debug.Log((ItemMaster.ItemList)_itemCode);
    }
}