using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : Item, IItemFunction
{
    public IEnumerator CarryItem(ItemSlot itemSlot)
    {
        if (MouseCursor.Instance.RightClickVoid)
        {
            // itemSlot.SetItem(ItemName.FISHING_ROD_USED);
        }
        yield break;
    }

    public IEnumerator MountItem()
    {
        yield break;
    }

    public IEnumerator InSlotItem()
    {
        yield break;
    }

    public IEnumerator UseItem<T>(T xValue) where T : IInteraction
    {
        yield break;
    }

    private void Awake()
    {
        _itemCode = (int)ItemName.FISHING_ROD;

        _itemType = ItemTypeList.TOOL;
    }

    public IEnumerator UnmountItem()
    {
        yield break;
    }

    public bool HasFunction(ItemInterface func)
    {
        switch (func)
        {
            case ItemInterface.Equip:
                return true;
        }
        return false;
    }
}
