using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_cutlassFish : Item
{
    protected override void Init()
    {
        _itemCode = (int)ItemMaster.ItemList.FISH_CUTLASSFISH;

        _itemType = ItemMaster.ItemType.FISH;
    }
}
