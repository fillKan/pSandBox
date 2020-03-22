using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IMouseAction
{
    private LinkedList<ItemList> _itemContainer = new LinkedList<ItemList>();
    public int ItemCount
    {
        get { return _itemContainer.Count; }
    }
    public Item ContainItem
    {
        get 
        {
            if(_itemContainer.Count != 0)
            {
                return ItemMaster.Instance.GetItem(_itemContainer.First.Value);
            }
            return null;
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
    public void AddItem(params ItemList[] items)
    {
        int i = 0;

        if (ItemCount == 0)
        {
            _itemContainer.AddLast(items[i++]);
        }

        for (; i < items.Length; i++)
        {
            if (_itemContainer.First.Value == items[i])
            {
                _itemContainer.AddLast(items[i]);
            }
            else Debug.LogWarning("적합하지 않은 아이템은 추가할 수 없습니다.");
        }
        UpdateSlotInfo();
    }
    public void AddItem(ItemList item)
    {
        if (_itemContainer.Count == 0)
        {
            _itemContainer.AddLast(item);
        }

        else if (item == _itemContainer.First.Value)
        {
            _itemContainer.AddLast(item);
        }
        UpdateSlotInfo();
    }

    #region 함수 설명 : 
    /// <summary>
    /// 해당하는 아이템 슬롯이 포함하는 아이템 종류와 갯수를 설정합니다.
    /// </summary>
    /// <param name="item">
    /// 해당 아이템슬롯이 포함할 아이템의 종류
    /// </param>
    /// <param name="number">
    /// 해당 아이템 슬롯이 포함할 아이템의 갯수
    /// </param>
    #endregion
    public void SetItem(ItemList item, int number = 1)
    {
        _itemContainer.Clear();

        for(int i = 0; i < number; i++)
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
            SlotSprt.HideItemExisting();
        }
        else 
        {
            SlotSprt.ShowItemExisting(ContainItem.ItemData);
        }
    }


    /******************************************
            IMouseAction 인터페이스 함수들
     ******************************************/
    public void OperateAction(byte input)
    {
        switch (input)
        {
            case 0:
                if (MouseCursor.Instance.CarryItem != ItemList.NONE)
                {
                    if (ContainItem == null)
                    {
                        _itemContainer.AddLast(MouseCursor.Instance.CarryItem);
                        MouseCursor.Instance.DelCarryItem();
                        UpdateSlotInfo();
                    }
                    else if (MouseCursor.Instance.CarryItem == ContainItem.ItemData)
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
                    if (MouseCursor.Instance.CarryItem == ItemList.NONE)
                    {
                        MouseCursor.Instance.AddCarryItem(_itemContainer.Last.Value);

                        _itemContainer.RemoveLast();
                        UpdateSlotInfo();
                    }
                    else if(ContainItem)
                    {
                        if (MouseCursor.Instance.CarryItem == ContainItem.ItemData)
                        {
                            MouseCursor.Instance.AddCarryItem(_itemContainer.Last.Value);

                            _itemContainer.RemoveLast();
                            UpdateSlotInfo();
                        }
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