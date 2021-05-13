using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_pufferFish : Item
{
    protected override void Init()
    {
        _itemCode = (int)ItemName.FISH_PUFFERFISH;

        _itemType = ItemTypeList.FISH;
    }
}
