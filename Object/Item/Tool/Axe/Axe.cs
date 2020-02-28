using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Item, ItemFunction
{
    protected override void Init()
    {
        _itemCode = (int)ItemMaster.ItemList.AXE;
    }

    public IEnumerator UseItem<T> (T xValue) where T : Interaction
    {
        if (xValue.InteractObject().TryGetComponent<Tree>(out Tree tree))
        {
            if (tree.DoingChopTree || tree.IsCutDown) yield break;

            yield return StartCoroutine(tree.CR_chopTree(4, 0.4f, 0.1f));

            if(tree.fDurability <= 0)
            {
                tree.DropItem();
                yield return StartCoroutine(tree.CR_cutDown());
            }
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
