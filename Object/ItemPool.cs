using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : Singleton<ItemPool>
{
    private Dictionary<int, Item> m_items = new Dictionary<int, Item>();

    public Item[] itemList;

    private void Awake()
    {
        for(int i = 0; i < itemList.Length; i++)
        {
            m_items.Add(itemList[i].itemCode, itemList[i]);
        }
    }

    public Item ItemInstantiate(int itemCode)
    {
        return null;
    }

    public void PlayerDetectionCheck()
    {
        foreach (Item item in m_items.Values)
        {
            
        }
    }
}
