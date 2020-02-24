using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_cutlassFish : Item
{
    protected override void Init()
    {
        _itemCode = (int)ItemMaster.ItemList.Fish_CUTLASSFISH;
    }
    public override void UseItem()
    {
        Debug.Log((ItemMaster.ItemList)_itemCode);
    }
}
