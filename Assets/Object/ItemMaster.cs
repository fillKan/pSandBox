using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 열거체 설명 : 
/// <summary>
/// 아이템들의 종류를 열거하는 열거체.
/// <para>
/// 해당 열거체를 통해 아이템을 구분한다.
/// </para>
/// </summary>
#endregion
public enum ItemList
{
    NONE,
    LOG_WHITEBIRCH,
    LOG_OAK,
    SEED_WHITEBIRCH,
    SEED_OAK,
    WOOL,
    EGG,
    FISH_BUTTERFISH,
    FISH_CUTLASSFISH,
    FISH_MACKEREL,
    FISH_POLLACK,
    FISH_PUFFERFISH,
    FISH_SALMON,
    AXE,
    FISHING_ROD,
    FISHING_ROD_USED,
    AXE_ELECTRIC_SAW,
    AXE_ORC_HANDAXE
};

#region 열거체 설명 : 
/// <summary>
/// 아이템들의 유형들을 열거하는 열거체.
/// <para>
/// 해당 열거체를 통해 아이템의 유형이 구분된다.
/// </para>
/// </summary>
#endregion
public enum ItemTypeList
{
    NONE,
    TOOL,
    FISH
}

#region 클래스 설명 : 
/// <summary>
/// 아이템과 관련된 모든 정보를 다루는 싱글톤 객체.
/// </summary>
#endregion
public class ItemMaster : Singleton<ItemMaster>
{
    #region 딕셔너리 설명 : 
    /// <summary>
    /// '기능을 하는' 아이템들의 정보를 저장하는 딕셔너리.
    /// </summary>
    #endregion
    private Dictionary<int, Item>   Items    = new Dictionary<int, Item>();
    #region 딕셔너리 설명 : 
    /// <summary>
    /// 아이템들의 스프라이트를 저장하는 딕셔너리.
    /// </summary>
    #endregion
    private Dictionary<int, Sprite> ItemSprs = new Dictionary<int, Sprite>();

    #region 딕셔너리 설명 : 
    /// <summary>
    /// '게임에서 보여지는' 아이템들의 정보를 저장하는 딕셔너리.
    /// </summary>
    #endregion
    private Dictionary<int, DroppedItem> ItemExistings = new Dictionary<int, DroppedItem>();
    #region 딕셔너리 설명 : 
    /// <summary>
    /// 생성된 '게임에서 보여지는' 아이템들을 풀링하는 딕셔너리.
    /// </summary>
    #endregion
    private Dictionary<int, Stack<DroppedItem>> ItemPool = new Dictionary<int, Stack<DroppedItem>>();

    private Dictionary<int, Item>.ValueCollection ItemValues;

    private List<int> FishItems = new List<int>();
    

    #region 함수 설명 : 
    /// <summary>
    /// '게임에서 보여지는' 아이템을 ItemMaster에 등록시키는 함수.
    /// </summary>
    /// <param name="item">
    /// 등록할 '게임에서 보여지는' 아이템 객체
    /// </param>
    #endregion
    public void Registration(DroppedItem item)
    {
        if (!ItemExistings.ContainsKey(item.ItemCode))
        {
            ItemExistings.Add(item.ItemCode, item);
        }
    }
    #region 함수 설명 : 
    /// <summary>
    /// '기능을 하는' 아이템과 그 아이템의 스프라이트를 ItemMaster에 등록시키는 함수.
    /// </summary>
    /// <param name="item">
    /// 등록할 '기능을 하는' 아이템 객체. 해당 객체에서 스프라이트를 추출한다.
    /// </param>
    #endregion
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

    #region 함수 설명 : 
    /// <summary>
    /// ItemMaster에 등록된 '게임에서 보여지는' 아이템을 반환하는 함수.
    /// </summary>
    /// <param name="itemCode">
    /// 반환을 원하는 아이템의 아이템 코드
    /// </param>
    /// <returns>
    /// 인자와 일치하는 아이템 코드를 가진 아이템을 반환한다.
    /// </returns>
    #endregion
    public DroppedItem GetItemExisting(int itemCode)
    {
        if (ItemExistings.ContainsKey(itemCode))
        {
            return ItemExistings[itemCode];
        }
        return null;
    }
    #region 함수 설명 : 
    /// <summary>
    /// ItemMaster에 등록된 '게임에서 보여지는' 아이템을 반환하는 함수.
    /// </summary>
    /// <param name="item">
    /// 반환을 원하는 아이템의 ItemList 열거자
    /// </param>
    /// <returns>
    /// 인자와 일치하는 아이템 데이터를 가진 아이템을 반환한다.
    /// </returns>
    #endregion
    public DroppedItem GetItemExisting(ItemList item)
    {
        int itemCode = (int)item;

        if (ItemExistings.ContainsKey(itemCode))
        {
            return ItemExistings[itemCode];
        }
        return null;
    }

    #region 함수 설명 : 
    /// <summary>
    /// '게임에서 보여지는' 아이템들의 풀에담긴 요소를 반환하는 함수.
    /// <para>
    /// 아이템 풀이 비어있거나, 존재하지 않는다면 ItemMaster에 등록된 아이템을 토대로 아이템을 하나 생성하여 반환한다.
    /// </para>
    /// </summary>
    /// <param name="itemCode">
    /// 반환을 원하는 아이템의 아이템 코드
    /// </param>
    /// <returns>
    /// 인자와 일치하는 아이템 코드를 가진 아이템을 반환한다.
    /// </returns>
    #endregion
    public DroppedItem TakeItemExisting(int itemCode)
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
    #region 함수 설명 : 
    /// <summary>
    /// '게임에서 보여지는' 아이템들의 풀에담긴 요소를 반환하는 함수.
    /// <para>
    /// 아이템 풀이 비어있거나, 존재하지 않는다면 ItemMaster에 등록된 아이템을 토대로 아이템을 하나 생성하여 반환한다.
    /// </para>
    /// </summary>
    /// <param name="item">
    /// 반환을 원하는 아이템의 아이템 열거자
    /// </param>
    /// <returns>
    /// 인자와 일치하는 아이템 데이터를 가진 아이템을 반환한다.
    /// </returns>
    #endregion
    public DroppedItem TakeItemExisting(ItemList item)
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

    #region 함수 설명 : 
    /// <summary>
    /// '게임에서 보여지는' 아이템들의 풀에 요소를 추가하는 함수.
    /// <para>
    /// 아이템 풀이 존재하지 않는다면 새로운 풀을 만들어낸다.
    /// </para>
    /// </summary>
    /// <param name="item">
    /// '게임에서 보여지는' 아이템들의 풀에 추가할 요소
    /// </param>
    #endregion
    public void StoreItemExisting(DroppedItem item)
    {
        if (ItemPool.ContainsKey(item.ItemCode))
        {
            ItemPool[item.ItemCode].Push(item);
        }
        else
        {
            ItemPool.Add(item.ItemCode, new Stack<DroppedItem>());

            ItemPool[item.ItemCode].Push(item);
        }
    }

    #region 함수 설명 : 
    /// <summary>
    /// '기능을 하는' 아이템을 반환하는 함수.
    /// </summary>
    /// <param name="itemCode">
    /// 반환을 원하는 아이템의 아이템 코드
    /// </param>
    /// <returns>
    /// 인자와 일치하는 아이템 코드를 가진 아이템을 반환한다.
    /// </returns>
    #endregion
    public Item GetItem(int itemCode)
    {
        if(Items.ContainsKey(itemCode))
        {
            return Items[itemCode];
        }
        return null;
    }
    #region 함수 설명 : 
    /// <summary>
    /// '기능을 하는' 아이템을 반환하는 함수.
    /// </summary>
    /// <param name="item">
    /// 반환을 원하는 아이템의 ItemList 열거자
    /// </param>
    /// <returns>
    /// 인자와 일치하는 아이템 데이터를 가진 아이템을 반환한다.
    /// </returns>
    #endregion
    public Item GetItem(ItemList item)
    {
        if (Items.ContainsKey((int)item))
        {
            return Items[(int)item];
        }
        return null;
    }

    #region 함수 설명 : 
    /// <summary>
    /// 무작위 물고기아이템의 아이템 코드를 반환하는 함수.
    /// </summary>
    #endregion
    public int RandomFish()
    {
        return FishItems[Random.Range(0, FishItems.Count)];
    }

    #region 함수 설명 : 
    /// <summary>
    /// 특정 아이템의 스프라이트를 반환하는 함수.
    /// </summary>
    /// <param name="itemCode">
    /// 반환을 원하는 아이템 스프라이트의 아이템 코드
    /// </param>
    /// <returns>
    /// 인자와 일치하는 아이템 코드를 가진 스프라이트를 반환한다.
    /// </returns>
    #endregion
    public Sprite GetItemSprt(int itemCode)
    {
        if (ItemSprs.ContainsKey(itemCode))
        {
            return ItemSprs[itemCode];
        }
        return null;
    }
    #region 함수 설명 : 
    /// <summary>
    /// 특정 아이템의 스프라이트를 반환하는 함수.
    /// </summary>
    /// <param name="item">
    /// 반환을 원하는 아이템 스프라이트의 ItemList 열거자
    /// </param>
    /// <returns>
    /// 인자와 일치하는 아이템 데이터를 가진 스프라이트를 반환한다.
    /// </returns>
    #endregion
    public Sprite GetItemSprt(ItemList item)
    {
        if (ItemSprs.ContainsKey((int)item))
        {
            return ItemSprs[(int)item];
        }
        return null;
    }


    private void Start()
    {
        ItemValues = Items.Values;

        foreach (Item item in ItemValues)
        {
            if(item.ItemType.Equals(ItemTypeList.FISH))
            {
                FishItems.Add(item.ItemCode);
            }
        }

    }
}
