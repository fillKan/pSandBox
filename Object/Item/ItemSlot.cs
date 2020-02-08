using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    private Stack<Item> _itemContainer = new Stack<Item>();
    
    public int ItemCount
    {
        get { return _itemContainer.Count; }
    }
    public Item ContainItem
    {
        get { return _itemContainer.Peek(); }
    }

    public Text text;

    public void AddItem(params Item[] items)
    {
        int i = 0;

        if (ItemCount == 0)
        {
            _itemContainer.Push(items[i++]);
        }

        for (; i < items.Length; i++)
        {
            if (_itemContainer.Peek() == items[i])
            {
                _itemContainer.Push(items[i]);
            }
            else Debug.LogWarning("적합하지 않은 아이템은 추가할 수 없습니다.");
        }
        UpdateItemCount();
    }

    public void AddItem(Item item)
    {
        if (_itemContainer.Count == 0)
        {
            _itemContainer.Push(item);
        }

        else if (item == _itemContainer.Peek())
        {
            _itemContainer.Push(item);          
        }
        UpdateItemCount();
    }

    public void UpdateItemCount()
    {
        text.text = ItemCount.ToString();
    }
}
