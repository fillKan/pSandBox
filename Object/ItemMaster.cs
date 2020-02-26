using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaster : Singleton<ItemMaster>
{
    private Dictionary<int, Item>   Items    = new Dictionary<int, Item>();
    private Dictionary<int, Sprite> ItemSprs = new Dictionary<int, Sprite>();
    private Dictionary<int, ItemFunction> ItemFunctions = new Dictionary<int, ItemFunction>();

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
        if(!ItemFunctions.ContainsKey(item.ItemCode))
        {
            if(item.TryGetComponent<ItemFunction>(out ItemFunction function))
            {
                ItemFunctions.Add(item.ItemCode, function);
            }
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
    
    public ItemFunction GetItemFunction(int itemCode)
    {
        if(ItemFunctions.ContainsKey(itemCode))
        {
            return ItemFunctions[itemCode];
        }
        return null;
    }
    public ItemFunction GetItemFunction(ItemList item)
    {
        if (ItemFunctions.ContainsKey((int)item))
        {
            return ItemFunctions[(int)item];
        }
        return null;
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

    public void UseItem(int itemCode)
    {
        if(Items.ContainsKey(itemCode))
        {
            //Items[itemCode].UseItem();
        }
    }
    public void UseItem(ItemList item)
    {
        if (Items.ContainsKey((int)item))
        {
           // Items[(int)item].UseItem();
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
