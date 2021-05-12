using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_cutlassFish : Item
{
    protected override void Init()
    {
        _itemCode = (int)ItemList.FISH_CUTLASSFISH;

        _itemType = ItemTypeList.FISH;
    }
}
