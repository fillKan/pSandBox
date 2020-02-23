using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, MouseAction
{
    private LinkedList<ItemMaster.ItemList> _itemContainer = new LinkedList<ItemMaster.ItemList>();
    private bool IsSlotEmpty = true;
    public int ItemCount
    {
        get { return _itemContainer.Count; }
    }
    public ItemMaster.ItemList ContainItem
    {
        get
        {
            if (_itemContainer.Count == 0) return ItemMaster.ItemList.NONE;

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
    public void AddItem(params ItemMaster.ItemList[] items)
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
    public void AddItem(ItemMaster.ItemList item)
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

    public void UpdateSlotInfo()
    {
        text.text = ItemCount.ToString();

        if (ContainItem == ItemMaster.ItemList.NONE)
        {
            SlotSprt.HideItemSprt();
            IsSlotEmpty = true;
        }
        else if (IsSlotEmpty)
        {
            SlotSprt.ShowItemSprt(ContainItem);
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
                if (MouseCursor.Instance.CarryItem != ItemMaster.ItemList.NONE)
                {
                    if (ContainItem == ItemMaster.ItemList.NONE)
                    {
                        _itemContainer.AddLast(MouseCursor.Instance.CarryItem);
                        MouseCursor.Instance.DelCarryItem();
                        UpdateSlotInfo();
                    }
                    else if (MouseCursor.Instance.CarryItem == ContainItem)
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
                    if (MouseCursor.Instance.CarryItem == ItemMaster.ItemList.NONE)
                    {
                        MouseCursor.Instance.AddCarryItem(_itemContainer.Last.Value);

                        _itemContainer.RemoveLast();
                        UpdateSlotInfo();
                    }
                    else if (MouseCursor.Instance.CarryItem == ContainItem)
                    {
                        MouseCursor.Instance.AddCarryItem(_itemContainer.Last.Value);

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