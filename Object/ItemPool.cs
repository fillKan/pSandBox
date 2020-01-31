using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : Singleton<ItemPool>
{
    private Dictionary<int, GameObject> m_items = new Dictionary<int, GameObject>();

    public GameObject[] itemList;
    public enum ItemList
    {
        LOG_WHITEBIRCH,
        LOG_OAK
    };

    public void AddItem(GameObject item)
    {
        if (item.TryGetComponent(out Item _item))
        {
            if(!m_items.ContainsKey(_item.itemCode))
            {
                m_items.Add(_item.itemCode, item);
            }
            
        }
    }

    public GameObject GetItem(int itemCode)
    {
        if (m_items.ContainsKey(itemCode))
        {
            return m_items[itemCode];
        }
        return null;
    }
    public GameObject GetItem(ItemList item)
    {
        if (m_items.ContainsKey((int)item))
        {
            return m_items[(int)item];
        }
        return null;
    }
}
