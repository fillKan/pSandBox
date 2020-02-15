using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [HideInInspector]
    public int itemCode
    {
        get { return _itemCode; }
    }
    protected int _itemCode;

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
    public void EnterContainer(Transform parent)
    {
        transform.parent = parent;
        transform.localPosition = Vector2.zero;
        transform.localScale = new Vector3(0.7f, 0.7f, 1);

        gameObject.SetActive(true);
        TryGetComponent<SpriteRenderer>(out SpriteRenderer spr);
        TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody);
        TryGetComponent<BoxCollider2D>(out BoxCollider2D box);
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        
        box.enabled = false;
        spr.sortingLayerID = FindObjectOfType<ItemSlot>().GetComponent<SpriteRenderer>().sortingLayerID;
        spr.sortingOrder = 1;
    }

    #region 함수 설명 : 
    /// <summary>
    /// 해당 아이템이 아이템 슬롯에 띄워지는 스프라이트로써의 역할이 끝났을 때에 실행하는 함수.
    /// </summary>
    #endregion
    public void ExitContainer()
    {
        transform.parent = null;
        transform.localScale = Vector3.one;

        gameObject.SetActive(false);
        TryGetComponent<SpriteRenderer>(out SpriteRenderer spr);
        TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody);
        TryGetComponent<BoxCollider2D>(out BoxCollider2D box);
        rigidbody.bodyType = RigidbodyType2D.Dynamic;

        box.enabled = true;
        spr.sortingLayerID = ItemMaster.Instance.GetItem(ItemMaster.ItemList.WOOL).GetComponent<SpriteRenderer>().sortingLayerID;
        spr.sortingOrder = 0;
    }
}
