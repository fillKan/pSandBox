﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_butterFish : Item
{
    protected override void Init()
    {
        _itemCode = (int)ItemMaster.ItemList.FISH_BUTTERFISH;

        _itemType = ItemMaster.ItemType.FISH;
    }
}
