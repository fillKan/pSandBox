using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaster : Singleton<ItemMaster>
{
    private Dictionary<int, Item> items = new Dictionary<int, Item>();

    public enum ItemList
    {
        LOG_WHITEBIRCH,
        LOG_OAK,
        SEED_WHITEBIRCH,
        SEED_OAK
    };

    public void Registration(Item item)
    {
        if (!items.ContainsKey(item.itemCode))
        {
            items.Add(item.itemCode, item);
        }
    }

    public Item GetItem(int itemCode)
    {
        if (items.ContainsKey(itemCode))
        {
            return items[itemCode];
        }
        return null;
    }
    public Item GetItem(ItemList item)
    {
        if (items.ContainsKey((int)item))
        {
            return items[(int)item];
        }
        return null;
    }
}
