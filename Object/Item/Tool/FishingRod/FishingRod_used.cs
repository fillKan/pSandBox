using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod_used : Item, ItemFunction
{
    [Tooltip("현재 낚시대의 낚시줄을 지정합니다.")]
    public LineRenderer FishingLine;

    [Tooltip("현재 낚시대의 낚시찌를 지정합니다.")]
    public Bobber bobber;

    #region 변수 설명 : 
    /// <summary>
    /// 현재 낚시찌를 던졌는지의 여부를 저장합니다.
    /// </summary>
    #endregion
    private bool isThrowBobber;
    #region 변수 설명 : 
    /// <summary>
    /// 낚시찌가 나아갈 방향을 저장합니다.
    /// </summary>
    #endregion
    private Vector2 vDir = Vector2.zero;
    #region 변수 설명 : 
    /// <summary>
    /// 현재 낚시대의 맨 끝 지점을 반환합니다.
    /// </summary>
    #endregion
    private Vector2 vRodTopPoint
    {
        get
        {
            Vector2 pos = PlayerGetter.Instance.player.CarryItem.transform.position;

            if (PlayerGetter.Instance.player.FlipX)
            {
                 pos.Set(pos.x - 1.3f, pos.y + 0.525f);
            }
            else pos.Set(pos.x + 1.3f, pos.y + 0.525f);

            return pos;
        }
    }

    public IEnumerator CarryItem(ItemSlot itemSlot)
    {
        if (!StartWorking(ref _isCarryItem)) yield break;

        if(!isThrowBobber && MouseCursor.Instance.ClickVoid)
        {
            FishingLine.enabled = true;
            isThrowBobber       = true;

            vDir = (Vector2)MouseCursor.Instance.transform.position - PlayerGetter.Instance.GetPos();

            bobber.gameObject.SetActive(true);

            bobber.GetRigidbody2D.velocity = Vector2.zero;
            bobber.GetRigidbody2D.AddForce(vDir.normalized * vDir.magnitude * 4.5f);

            bobber.transform.position = vRodTopPoint;

            FishingLine.SetWidth(0.05f, 0.05f);
        }
        else if (isThrowBobber && MouseCursor.Instance.ClickVoid)
        {
            itemSlot.SetItem(ItemMaster.ItemList.FISHING_ROD);

            bobber.CatchFish();

            vDir = vRodTopPoint - (Vector2)bobber.transform.position;

            bobber.GetRigidbody2D.velocity = Vector2.zero;
            bobber.GetRigidbody2D.AddForce(vDir.normalized * vDir.magnitude * 4.5f);

            FishingLine.enabled = false;
            isThrowBobber       = false;         
        }
        if (isThrowBobber)
        {
            FishingLine.SetPosition(0, vRodTopPoint);
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
