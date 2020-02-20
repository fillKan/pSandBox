using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region 클래스 설명 : 
/// <summary>
/// 아이템 슬롯에 담긴 아이템을 보여주는데 사용되는 오브젝트의 각종 정보를 담는다. 
/// </summary>
#endregion
public class SlotSpriteInfo
{
    #region 설명 : 
    /// <summary>
    /// 아이템 슬롯에서 보여지는 오브젝트의 SpriteRenderer를 담는다.
    /// </summary>
    #endregion
    public SpriteRenderer Spr;
    #region 설명 : 
    /// <summary>
    /// 아이템 슬롯에서 보여지는 오브젝트의 Rigidbody2D를 담는다.
    /// </summary>
    #endregion
    public Rigidbody2D Rigidbody;
    #region 설명 : 
    /// <summary>
    /// 아이템 슬롯에서 보여지는 오브젝트의 BoxCollider2D를 담는다.
    /// </summary>
    #endregion
    public BoxCollider2D Box;

    #region 설명 : 
    /// <summary>
    /// 아이템 슬롯의 SpriteRenderer.sortingLayerID의 값을 담는다.
    /// </summary>
    #endregion
    public int SortingLayerID
    {
        get { return _sortingLayerID; }
    }
    private int _sortingLayerID;

    #region 생성자 설명 : 
    /// <summary>
    /// SortingLayerID의 값을 설정하고, 그 외 모든 파라미터의 값을 null로 초기화한다.
    /// </summary>
    /// <param name="layerID">
    /// 설정할 SortingLayerID의 값.
    /// <para>
    /// 아이템 슬롯의 SpriteRenderer.sortingLayerID의 값을 담는다.
    /// </para>
    /// </param>
    #endregion
    public SlotSpriteInfo(int layerID)
    {
        this._sortingLayerID = layerID;

        this.Rigidbody = null;
        this.Spr = null;
        this.Box = null;
    }

    #region 함수 설명 : 
    /// <summary>
    /// SortingLayerID를 제외한 모든 파라미터의 값을 초기화한다.
    /// </summary>
    #endregion
    public void Init()
    {
        this.Rigidbody = null;
        this.Spr = null;
        this.Box = null;
    }
}

public class ItemSlot : MonoBehaviour, MouseAction
{
    private LinkedList<Item> _itemContainer = new LinkedList<Item>();
    private bool IsSlotEmpty = true;
    public int ItemCount
    {
        get { return _itemContainer.Count; }
    }
    public Item ContainItem
    {
        get
        {
            if (_itemContainer.Count == 0) return null;

            return _itemContainer.First.Value;
        }
    }

    public Text text;
    public ItemSlotSprt SlotSprt;

    private void Awake()
    {
        RegisterAction();
    }

    private void OnEnable()
    {
        UpdateSlotInfo();
    }

    /***************************************
           ItemSlot를 관리하는 함수들
    ****************************************/
    public void AddItem(params Item[] items)
    {
        int i = 0;

        if (ItemCount == 0)
        {
            _itemContainer.AddLast(items[i++]);
        }

        for (; i < items.Length; i++)
        {
            if (_itemContainer.First.Value.itemCode == items[i].itemCode)
            {
                _itemContainer.AddLast(items[i]);
            }
            else Debug.LogWarning("적합하지 않은 아이템은 추가할 수 없습니다.");
        }
        UpdateSlotInfo();
    }
    public void AddItem(Item item)
    {
        if (_itemContainer.Count == 0)
        {
            _itemContainer.AddLast(item);
        }

        else if (item.itemCode == _itemContainer.First.Value.itemCode)
        {
            _itemContainer.AddLast(item);
        }
        UpdateSlotInfo();
    }

    public void UpdateSlotInfo()
    {
        text.text = ItemCount.ToString();

        if (ContainItem == null)
        {
            SlotSprt.HideItemSprt();
            IsSlotEmpty = true;
        }
        else if (IsSlotEmpty)
        {
            SlotSprt.ShowItemSprt(ContainItem.itemCode);
            IsSlotEmpty = false;
        }
    }


    /******************************************
            MouseAction 인터페이스 함수들
     ******************************************/
    public void OperateAction(byte input)
    {
        switch (input)
        {
            case 0:
                if (MouseCursor.Instance.CarryItem != null)
                {
                    if (ContainItem == null)
                    {
                        _itemContainer.AddLast(MouseCursor.Instance.CarryItem);
                        MouseCursor.Instance.DelCarryItem();
                        UpdateSlotInfo();
                    }
                    else if (MouseCursor.Instance.CarryItem.itemCode == ContainItem.itemCode)
                    {
                        _itemContainer.AddLast(MouseCursor.Instance.CarryItem);
                        MouseCursor.Instance.DelCarryItem();
                        UpdateSlotInfo();
                    }
                }
                break;

            case 1:
                if (ItemCount > 0)
                {
                    if (MouseCursor.Instance.CarryItem == null)
                    {
                        MouseCursor.Instance.AddCarryItem(_itemContainer.Last.Value);

                        if (ContainItem.Equals(_itemContainer.Last.Value))
                        {
                            //SlotSprt.HideItemSprt();
                        }

                        _itemContainer.RemoveLast();
                        UpdateSlotInfo();
                    }
                    else if (MouseCursor.Instance.CarryItem.itemCode == ContainItem.itemCode)
                    {
                        MouseCursor.Instance.AddCarryItem(_itemContainer.Last.Value);

                        if (ContainItem.Equals(_itemContainer.Last.Value))
                        {
                            //SlotSprt.HideItemSprt();
                        }

                        _itemContainer.RemoveLast();
                        UpdateSlotInfo();
                    }
                }
                break;
        }
    }

    public void RegisterAction()
    {
        MouseRepeater.Instance.AddActionObj(gameObject.GetInstanceID(), this);
    }

    public GameObject ActionObject()
    {
        return gameObject;
    }
}