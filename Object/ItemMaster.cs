using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaster : Singleton<ItemMaster>
{
    private Dictionary<int, Item>   Items    = new Dictionary<int, Item>();
    private Dictionary<int, Sprite> ItemSprs = new Dictionary<int, Sprite>();

    public enum ItemList
    {
        LOG_WHITEBIRCH,
        LOG_OAK,
        SEED_WHITEBIRCH,
        SEED_OAK,
        WOOL,
        EGG,
        Fish_BUTTERFISH,
        Fish_CUTLASSFISH,
        FISH_MACKEREL,
        FISH_POOLLACK,
        FISH_PUFFERFISH,
        FISH_SALMON,
        AXE
    };

    public void Registration(Item item)
    {
        if (!Items.ContainsKey(item.itemCode))
        {
            Items.Add(item.itemCode, item);

            item.TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer);

            ItemSprs.Add(item.itemCode, renderer.sprite);
        }
    }

    public Item GetItem(int itemCode)
    {
        if (Items.ContainsKey(itemCode))
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

}
