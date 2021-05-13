using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_pollack : Item
{
    protected override void Init()
    {
        _itemCode = (int)ItemName.FISH_POLLACK;

        _itemType = ItemTypeList.FISH;
    }
}
