using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_butterFish : Item
{
    protected override void Init()
    {
        _itemCode = (int)ItemName.FISH_BUTTERFISH;

        _itemType = ItemTypeList.FISH;
    }
}
