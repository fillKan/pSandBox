using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaster : Singleton<ItemMaster>
{
    private Dictionary<int, Item>   Items    = new Dictionary<int, Item>();
    private Dictionary<int, Sprite> ItemSprs = new Dictionary<int, Sprite>();

    private Dictionary<ItemList, Stack<ItemExisting>> ItemExistings = new Dictionary<ItemList, Stack<ItemExisting>>();

    public enum ItemList
    {
        NONE,
        LOG_WHITEBIRCH,
        LOG_OAK,
        SEED_WHITEBIRCH,
        SEED_OAK,
        WOOL,
        EGG,
        Fish_BUTTERFISH,
        Fish_CUTLASSFISH,
        FISH_MACKEREL,
        FISH_POLLACK,
        FISH_PUFFERFISH,
        FISH_SALMON,
        AXE
    };

    public void Registration(ItemExisting item)
    {
        if (!ItemExistings.ContainsKey(item.ItemCode))
        {
            item.TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer);

            ItemExistings.Add(item.ItemCode, new Stack<ItemExisting>());
            ItemExistings[item.ItemCode].Push(item);

            ItemSprs.Add((int)item.ItemCode, renderer.sprite);
        }
    }

    public void Registration(Item item)
    {
        if (!Items.ContainsKey((int)item.ItemCode))
        {
            Items.Add(item.ItemCode, item);
        }
    }

    public ItemExisting GetItemExisting(int itemCode)
    {
        ItemList item = (ItemList)itemCode;

        if (ItemExistings.ContainsKey(item))
        {
            if (ItemExistings[item].Count > 0)
            {
                return ItemExistings[item].Peek();
            }
        }
        return null;
    }
    public ItemExisting GetItemExisting(ItemList item)
    {
        if (ItemExistings.ContainsKey(item))
        {
            if (ItemExistings[item].Count > 0)
            {
                return ItemExistings[item].Peek();
            }
        }
        return null;
    }

    public ItemExisting TakeItemExisting(int itemCode)
    {
        ItemList item = (ItemList)itemCode;

        if (ItemExistings.ContainsKey(item))
        {
            if (ItemExistings[item].Count > 0)
            {
                return ItemExistings[item].Pop();
            }
        }
        return null;
    }
    public ItemExisting TakeItemExisting(ItemList item)
    {
        if (ItemExistings.ContainsKey(item))
        {
            if (ItemExistings[item].Count > 0)
            {
                return ItemExistings[item].Pop();
            }
        }
        return null;
    }
    public void StoreItemExisting(ItemExisting item)
    {
        if (ItemExistings.ContainsKey(item.ItemCode))
        {
            ItemExistings[item.ItemCode].Push(item);
        }
        else
        {
            ItemExistings.Add(item.ItemCode, new Stack<ItemExisting>());
            ItemExistings[item.ItemCode].Push(item);
        }
    }
    
    public void UseItem(int itemCode)
    {
        if(Items.ContainsKey(itemCode))
        {
            Items[itemCode].UseItem();
        }
    }

    public Sprite GetItemSprt(int itemCode)
    {
        if (ItemSprs.ContainsKey(itemCode))
        {
            return ItemSprs[itemCode];
        }
        return null;
    }

    public Sprite GetItemSprt(ItemList item)
    {
        if (ItemSprs.ContainsKey((int)item))
        {
            return ItemSprs[(int)item];
        }
        return null;
    }
}
