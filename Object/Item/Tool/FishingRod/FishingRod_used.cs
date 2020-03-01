using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod_used : Item, ItemFunction
{
    public LineRenderer FishingLine;

    private bool   isThrowBobber;
    public Bobber bobber;

    public IEnumerator CarryItem(ItemSlot itemSlot)
    {
        Debug.Log(MouseCursor.Instance.transform.position.normalized);
        if (!StartWorking(ref _isCarryItem)) yield break;

        if(!isThrowBobber && MouseCursor.Instance.ClickVoid)
        {
            Debug.Log("Throw!");
            isThrowBobber = true;

            Vector2 vDir = (Vector2)MouseCursor.Instance.transform.position - PlayerGetter.Instance.GetPos();
                    vDir.Normalize();

            bobber.transform.position = PlayerGetter.Instance.GetPos();
            bobber.GetRigidbody2D.AddForce(vDir * 20);
            Debug.Log(MouseCursor.Instance.transform.position.normalized);
            FishingLine.SetWidth(0.06f, 0.06f);
        }
        else if (isThrowBobber && MouseCursor.Instance.ClickVoid)
        {
            itemSlot.SetItem(ItemMaster.ItemList.FISHING_ROD);
            isThrowBobber = false;
        }
        if (isThrowBobber)
        {
            FishingLine.SetPosition(0, PlayerGetter.Instance.GetPos());
            FishingLine.SetPosition(1, bobber.transform.position);
        }
        

        StopWorking(ref _isCarryItem);
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

    public IEnumerator UseItem<T>(T xValue) where T : Interaction
    {
        yield break;
    }

    protected override void Init()
    {
        _itemCode = (int)ItemMaster.ItemList.FISHING_ROD_USED;

        _itemType = ItemMaster.ItemType.TOOL;
    }
}
