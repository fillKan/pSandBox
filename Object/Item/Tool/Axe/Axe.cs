using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Item, IItemFunction
{
    private float loggingValue = 3;

    protected override void Init()
    {
        _itemCode = (int)ItemMaster.ItemList.AXE;

        _itemType = ItemMaster.ItemType.TOOL;
    }

    public IEnumerator UseItem<T> (T xValue) where T : Interaction
    {
        yield break;
    }

    public IEnumerator CarryItem(ItemSlot itemSlot)
    {
        yield break;
    }

    public IEnumerator MountItem()
    {
        Debug.Log("Mount");
        StateStorage.Instance.IncreaseState(States.TREE_LOGGING, loggingValue);
        yield break;
    }

    public IEnumerator InSlotItem()
    {
        yield break;
    }

    public IEnumerator UnmountItem()
    {
        Debug.Log("Unmount");
        StateStorage.Instance.DecreaseState(States.TREE_LOGGING, loggingValue);
        yield break;
    }
}
