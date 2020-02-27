using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Item, ItemFunction
{
    private AxeModule axeModule;

    protected override void Init()
    {
        _itemCode = (int)ItemMaster.ItemList.AXE;
    }

    public IEnumerator UseItem<T> (T xValue) where T : Interaction
    {
        if (xValue.InteractObject().TryGetComponent<Tree>(out Tree value))
        {
            yield return StartCoroutine(value.CR_vibration(0.4f, 0.1f));
        }
        yield break;
    }

    public IEnumerator CarryItem()
    {
        yield break;
    }

    public IEnumerator EquipItem()
    {
        yield break;
    }

    public IEnumerator InSlotItem()
    {
        yield break;
    }
}
