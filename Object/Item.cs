using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [HideInInspector]
    public    int  itemCode
    {
        get { return _itemCode; }
    }
    protected int _itemCode;

    public    Rigidbody2D  Rigidbody
    {
        get { return _rigidbody; }
    }
    protected Rigidbody2D _rigidbody;

    protected abstract void Init();

    private void Awake()
    {
        Init();

        ItemMaster.Instance.Registration(this);
    }

    #region 함수 설명 : 
    /// <summary>
    /// 해당 아이템이 아이템 슬롯에 띄워지는 스프라이트로써 사용될 때에 실행하는 함수. 
    /// </summary>
    /// <param name="parent">
    /// 띄워질 아이템 슬롯의 transform.
    /// </param>
    #endregion
    public void EnterContainer(Transform parent, SlotSpriteInfo slotSprite)
    {
        transform.parent        = parent;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.localPosition = Vector2.zero;
        transform.localScale    = new Vector3(0.7f, 0.7f, 1);

        gameObject.SetActive(true);
        TryGetComponent<SpriteRenderer> (out slotSprite.Spr);
        TryGetComponent<BoxCollider2D>  (out slotSprite.Box);

        slotSprite.Rigidbody             = _rigidbody;
        slotSprite.Rigidbody.isKinematic = true;

        slotSprite.Box.enabled = false;
        slotSprite.Spr.sortingLayerID = slotSprite.SortingLayerID;
        slotSprite.Spr.sortingOrder   = 1;
    }

    #region 함수 설명 : 
    /// <summary>
    /// 해당 아이템이 아이템 슬롯에 띄워지는 스프라이트로써의 역할이 끝났을 때에 실행하는 함수.
    /// </summary>
    #endregion
    public void ExitContainer(SlotSpriteInfo slotSprite)
    {
        transform.parent     = null;
        transform.localScale = Vector3.one;

        gameObject.SetActive(false);
        slotSprite.Rigidbody.isKinematic = false;

        slotSprite.Box.enabled        = true;
        slotSprite.Spr.sortingLayerID = ItemMaster.Instance.GetItem(ItemMaster.ItemList.WOOL).GetComponent<SpriteRenderer>().sortingLayerID;
        slotSprite.Spr.sortingOrder   = 0;

        slotSprite.Init();
    }
}
