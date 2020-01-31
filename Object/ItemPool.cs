using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : Singleton<ItemPool>
{
    private Dictionary<int, Item> m_items = new Dictionary<int, Item>();

    public enum ItemList
    {
        LOG_WHITEBIRCH,
        LOG_OAK
    };

    public void AddItem(Item item)
    {
        if(!m_items.ContainsKey(item.itemCode))
        {
            m_items.Add(item.itemCode, item);
        }
    }

    public Item GetItem(int itemCode)
    {
        if (m_items.ContainsKey(itemCode))
        {
            return m_items[itemCode];
        }
        return null;
    }
    public Item GetItem(ItemList item)
    {
        if (m_items.ContainsKey((int)item))
        {
            return m_items[(int)item];
        }
        return null;
    }
}
