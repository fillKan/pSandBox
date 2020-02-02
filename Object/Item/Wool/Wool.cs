using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wool : Item
{
    protected override void Init()
    {
        _itemCode = (int)ItemMaster.ItemList.WOOL;
    }
}
