using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricSaw : Item
{
    protected override void Init()
    {
        _itemCode = (int)ItemList.AXE_ELECTRIC_SAW;

        _itemType = ItemTypeList.TOOL;
    }
}
