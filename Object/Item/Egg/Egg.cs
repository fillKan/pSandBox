using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : Item
{
    protected override void Init()
    {
        _itemCode = (int)ItemMaster.ItemList.EGG;
    }
    public override void UseItem()
    {
        Debug.Log((ItemMaster.ItemList)_itemCode);
    }
}
