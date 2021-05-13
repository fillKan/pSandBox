using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_mackerel : Item
{
    protected override void Init()
    {
        _itemCode = (int)ItemName.FISH_MACKEREL;

        _itemType = ItemTypeList.FISH;
    }
}
