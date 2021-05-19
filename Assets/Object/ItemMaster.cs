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
public enum ItemName
{
    NONE,
    LOG_WHITEBIRCH,
    LOG_OAK,
    SEED_WHITEBIRCH,
    SEED_OAK,
    WOOL,
    EGG,
    Fish_PampusArgenteus,
    FISH_CUTLASSFISH,
    FISH_MACKEREL,
    FISH_POLLACK,
    FISH_PUFFERFISH,
    FISH_SALMON,
    AXE,
    FISHING_ROD,
    AXE_ELECTRIC_SAW = 16,
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
    [Header("Item Collection")]
    [SerializeField] private ItemList _ItemList;
    [SerializeField] private ItemList _FishItemList;

    [Header("DroppedItem Collection")]
    [SerializeField] private DroppedItemList _DroppedItemList;

    private Dictionary<ItemName, Item> _ItemDic;
    private Dictionary<ItemName, Item> _ItemObjectDic;
    private Dictionary<ItemName, Queue<DroppedItem>> _DroppedItemPool;
    private Dictionary<ItemName, DroppedItem> _DroppedItemCollection;

    private List<ItemName> _FishList = new List<ItemName>();
    
    public DroppedItem GetDroppedItem(ItemName item)
    {
        if (_DroppedItemPool.ContainsKey(item))
        {
            if (_DroppedItemPool[item].Count > 0)
            {
                var dropped = _DroppedItemPool[item].Dequeue();
                    dropped.gameObject.SetActive(true);

                return dropped;
            }
        }
        return Instantiate(_DroppedItemCollection[item]);
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
    public void AddDroppedItem(DroppedItem item)
    {
        if(!_DroppedItemPool.ContainsKey(item.Name)) {
            _DroppedItemPool.Add(item.Name, new Queue<DroppedItem>());
        }
        _DroppedItemPool[item.Name].Enqueue(item);

        item.gameObject.SetActive(false);
        item.transform.rotation = Quaternion.identity;
        item.Rigidbody.velocity = Vector2.zero;
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
    public Item GetItem(ItemName item)
    {
        if (_ItemDic.ContainsKey(item))
        {
            return _ItemDic[item];
        }
        return null;
    }
    public Item GetItemObject(ItemName item)
    {
        Item returnValue;

        if (!_ItemObjectDic.TryGetValue(item, out returnValue))
        {
            returnValue = Instantiate(_ItemDic[item]);
            _ItemObjectDic.Add(item, returnValue);
        }
        returnValue.gameObject.SetActive(true);
        return returnValue;
    }
    public void AddItemObject(Item itemObject)
    {
        itemObject.gameObject.SetActive(false);
    }

    #region 함수 설명 : 
    /// <summary>
    /// 무작위 물고기아이템의 아이템 코드를 반환하는 함수.
    /// </summary>
    #endregion
    public ItemName RandomFish()
    {
        return _FishList[Random.Range(0, _FishList.Count)];
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
        return GetItemSprt((ItemName)itemCode);
    }
    public Sprite GetItemSprt(ItemName item)
    {
        if (_ItemDic.ContainsKey(item))
        {
            return _ItemDic[item].Sprite;
        }
        return null;
    }
    private void Awake()
    {
        _ItemDic = _ItemList.GetKeyValuePairs();
        _ItemObjectDic = new Dictionary<ItemName, Item>();

        _DroppedItemCollection = _DroppedItemList.GetKeyValuePairs();
        _DroppedItemPool = new Dictionary<ItemName, Queue<DroppedItem>>();

        _FishList = _FishItemList.GetList();
    }
}
