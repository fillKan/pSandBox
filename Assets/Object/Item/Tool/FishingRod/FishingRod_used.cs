using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod_used : Item, IItemFunction
{
    [Tooltip("현재 낚시대의 낚시줄을 지정합니다.")]
    public LineRenderer FishingLine;

    [Tooltip("현재 낚시대의 낚시찌를 지정합니다.")]
    public Bobber bobber;

    [Tooltip("낚시찌를 회수할 때, 낚시찌의 회수를 감지할 콜라이더를 지정합니다.")]
    public BoxCollider2D boxCollider;

    #region 변수 설명 : 
    /// <summary>
    /// 현재 낚시찌를 던졌는지의 여부를 저장합니다.
    /// </summary>
    #endregion
    private bool isThrowBobber;
    #region 변수 설명 : 
    /// <summary>
    /// 현재 낚시찌를 회수했는지의 여부를 저장합니다.
    /// </summary>
    #endregion
    private bool tryRetrieve;
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

    private ItemSlot slot;

    public IEnumerator CarryItem(ItemSlot itemSlot)
    {
        if (!isThrowBobber && MouseCursor.Instance.RightClickVoid)
        {
            isThrowBobber       = true;

            vDir = (Vector2)MouseCursor.Instance.transform.position - PlayerGetter.Instance.GetPos();

            bobber.gameObject.SetActive(true);

            bobber.GetRigidbody2D.velocity = Vector2.zero;
            bobber.GetRigidbody2D.AddForce(vDir.normalized * vDir.magnitude * 4.5f);

            bobber.transform.position = vRodTopPoint;
        }

        else if (isThrowBobber && MouseCursor.Instance.RightClickVoid)
        {
            slot = itemSlot;
            vDir = vRodTopPoint - (Vector2)bobber.transform.position;
            bobber.GetRigidbody2D.velocity = Vector2.zero;

            bobber.CatchFish(vDir.normalized * vDir.sqrMagnitude * 4.5f);

            tryRetrieve = true;
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

    public IEnumerator UnmountItem()
    {
        yield break;
    }

    private void Awake()
    {
        _itemCode = (int)ItemName.FISHING_ROD_USED;

        _itemType = ItemTypeList.TOOL;
    }

    private void Start()
    {
        StartCoroutine(CR_update());

        FishingLine.SetWidth(0.05f, 0.05f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(tryRetrieve)
        {
            if(other.gameObject.Equals(bobber.gameObject))
            {
                slot.SetItem(ItemName.FISHING_ROD);

                bobber.GetRigidbody2D.velocity = Vector2.zero;
                bobber.gameObject.SetActive(false);

                isThrowBobber       = false;
                tryRetrieve         = false;
                FishingLine.enabled = false;
            }
        }
    }

    private IEnumerator CR_update()
    {
        while(true)
        {
            if(isThrowBobber)
            {
                if(!FishingLine.enabled)
                {
                    FishingLine.enabled = true;
                }

                FishingLine.SetPosition(0, vRodTopPoint);
                FishingLine.SetPosition(1, bobber.transform.position);

                boxCollider.transform.position = vRodTopPoint;
            }
            yield return null;
        }
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
