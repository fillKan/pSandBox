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
        if (!StartWorking(ref _isCarryItem)) yield break;

        if(!isThrowBobber && MouseCursor.Instance.ClickVoid)
        {
            FishingLine.enabled = true;
            isThrowBobber       = true;

            Vector2 vDir = (Vector2)MouseCursor.Instance.transform.position - PlayerGetter.Instance.GetPos();
                    vDir.Normalize();

            bobber.gameObject.SetActive(true);

            bobber.GetRigidbody2D.velocity = Vector2.zero;
            bobber.GetRigidbody2D.AddForce(vDir * vDir.magnitude * 45);

            bobber.transform.position = PlayerGetter.Instance.player.CarryItem.transform.position;

            FishingLine.SetWidth(0.05f, 0.05f);
        }
        else if (isThrowBobber && MouseCursor.Instance.ClickVoid)
        {
            itemSlot.SetItem(ItemMaster.ItemList.FISHING_ROD);

            FishingLine.enabled = false;
            isThrowBobber       = false;

            bobber.gameObject.SetActive(false);
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
