using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : Singleton<ItemPool>
{
    private Dictionary<int, Item> m_items = new Dictionary<int, Item>();

    public Item[] itemList;
    public enum ItemList
    {
        LOG_WHITEBIRCH,
        LOG_OAK
    };

    private void Awake()
    {
        for(int i = 0; i < itemList.Length; i++)
        {
            m_items.Add(itemList[i].itemCode, itemList[i]);
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
