using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaster : Singleton<ItemMaster>
{
    private Dictionary<int, Item>   Items    = new Dictionary<int, Item>();
    private Dictionary<int, Sprite> ItemSprs = new Dictionary<int, Sprite>();

    private Dictionary<int, ItemExisting> ItemExistings = new Dictionary<int, ItemExisting>();
    private Dictionary<int, Stack<ItemExisting>> ItemPool = new Dictionary<int, Stack<ItemExisting>>();

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
            ItemExistings.Add(item.ItemCode, item);
        }
    }

    public void Registration(Item item)
    {
        if (!Items.ContainsKey((int)item.ItemCode))
        {
            Items.Add(item.ItemCode, item);
        }
        
        if(!ItemSprs.ContainsKey(item.ItemCode))
        {
            item.TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer);

            ItemSprs.Add((int)item.ItemCode, renderer.sprite);
        }
    }

    public ItemExisting GetItemExisting(int itemCode)
    {
        if (ItemExistings.ContainsKey(itemCode))
        {
            return ItemExistings[itemCode];
        }
        return null;
    }
    public ItemExisting GetItemExisting(ItemList item)
    {
        int itemCode = (int)item;

        if (ItemExistings.ContainsKey(itemCode))
        {
            return ItemExistings[itemCode];
        }
        return null;
    }

    public ItemExisting TakeItemExisting(int itemCode)
    {
        if (ItemPool.ContainsKey(itemCode))
        {
            if (ItemPool[itemCode].Count > 0)
            {
                return ItemPool[itemCode].Pop();
            }
        }

        // 만약 ItemPool에 키 값이 없거나, ItemPool이 비어있다면 여기로 오게된다.

        if (ItemExistings.ContainsKey(itemCode))
        {
            return Instantiate(ItemExistings[itemCode], Vector2.zero, Quaternion.identity);
        }
        return null;
    }
    public ItemExisting TakeItemExisting(ItemList item)
    {
        int itemCode = (int)item;

        if (ItemPool.ContainsKey(itemCode))
        {
            if (ItemPool[itemCode].Count > 0)
            {
                return ItemPool[itemCode].Pop();
            }
        }

        // 만약 ItemPool에 키 값이 없거나, ItemPool이 비어있다면 여기로 오게된다.

        if (ItemExistings.ContainsKey(itemCode))
        {
            return Instantiate(ItemExistings[itemCode], Vector2.zero, Quaternion.identity);
        }
        return null;
    }
    public void StoreItemExisting(ItemExisting item)
    {
        if (ItemPool.ContainsKey(item.ItemCode))
        {
            ItemPool[item.ItemCode].Push(item);
        }
        else
        {
            ItemPool.Add(item.ItemCode, new Stack<ItemExisting>());

            ItemPool[item.ItemCode].Push(item);
        }
    }

    public Item GetItem(int itemCode)
    {
        if(Items.ContainsKey(itemCode))
        {
            return Items[itemCode];
        }
        return null;
    }
    public Item GetItem(ItemList item)
    {
        if (Items.ContainsKey((int)item))
        {
            return Items[(int)item];
        }
        return null;
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
