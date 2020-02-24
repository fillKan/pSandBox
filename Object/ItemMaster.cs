using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaster : Singleton<ItemMaster>
{
    private Dictionary<int, ItemSprt>   Items = new Dictionary<int, ItemSprt>();
    private Dictionary<int, Sprite> ItemSprs  = new Dictionary<int, Sprite>();

    private Dictionary<ItemList, Stack<ItemSprt>> ItemSprts = new Dictionary<ItemList, Stack<ItemSprt>>();

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

    public void Registration(ItemSprt item)
    {
        if (!Items.ContainsKey((int)item.ItemCode))
        {
            Items.Add((int)item.ItemCode, item);

            item.TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer);

            ItemSprs.Add((int)item.ItemCode, renderer.sprite);
        }
    }

    public ItemSprt GetItem(int itemCode)
    {
        if (Items.ContainsKey(itemCode))
        {
            return Items[itemCode];
        }
        return null;
    }
    public ItemSprt GetItem(ItemList item)
    {
        if (Items.ContainsKey((int)item))
        {
            return Items[(int)item];
        }
        return null;
    }

    public Sprite GetItemSpr(int itemCode)
    {
        if (ItemSprs.ContainsKey(itemCode))
        {
            return ItemSprs[itemCode];
        }
        return null;
    }

    public Sprite GetItemSpr(ItemList item)
    {
        if (ItemSprs.ContainsKey((int)item))
        {
            return ItemSprs[(int)item];
        }
        return null;
    }

    public ItemSprt DropItem(ItemList item)
    {
        if(ItemSprts.ContainsKey(item))
        {
            if (ItemSprts[item].Count > 0)
            {
                return ItemSprts[item].Pop();
            }
        }
        Debug.LogError("Not Found the Value or Key");
        return null;
    }

    public void LoadItem(ItemSprt item)
    {
        if(ItemSprts.ContainsKey(item.ItemCode))
        {
            ItemSprts[item.ItemCode].Push(item);
        }
        else
        {
            ItemSprts.Add(item.ItemCode, new Stack<ItemSprt>());
            ItemSprts[item.ItemCode].Push(item);
        }
    }
}
