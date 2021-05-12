using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcHandAxe : Item
{
    protected override void Init()
    {
        _itemCode = (int)ItemList.AXE_ORC_HANDAXE;

        _itemType = ItemTypeList.TOOL;
    }
}
