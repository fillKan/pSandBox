using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Item, ItemFunction
{
    protected override void Init()
    {
        _itemCode = (int)ItemMaster.ItemList.AXE;
    }
    // 아이템한테 이렇게 많은 권한을 주어도 되는가 싶다
    public IEnumerator UseItem<T> (T xValue) where T : Interaction
    {
        if (xValue.InteractObject().TryGetComponent<Tree>(out Tree tree))
        {
            if (tree.DoingChopTree) yield break;

            tree.DoingChopTree = true;
            tree.fDurability -= 4;

            yield return StartCoroutine(tree.CR_vibration(0.4f, 0.1f));

            if(tree.fDurability <= 0)
            {
                tree.DropItem();
                yield return StartCoroutine(tree.CR_fade());
                tree.gameObject.SetActive(false);
            }
            tree.DoingChopTree = false;
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
