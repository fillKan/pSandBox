﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_salmon : Item
{
    protected override void Init()
    {
        _itemCode = (int)ItemName.FISH_SALMON;

        _itemType = ItemTypeList.FISH;
    }
}
