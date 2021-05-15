using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemInterface
{
    Use,
    Equip,
    MOUNT,
    UNMOUNT,
    INSLOT
}

#region 인터페이스 설명
/// <summary>
/// 아이템들의 기능하는 동작 코루틴들을 담는 인터페이스.
/// <para>※ 기능을 하는 아이템 클래스는 해당 인터페이스를 사용해야함. ※</para> 
/// </summary>
#endregion
public interface IItemFunction
{
    #region 코루틴 설명
    /// <summary>
    /// 플레이어가 해당 아이템을 들고, 오브젝트와 상호작용 했을 경우의 행동.
    /// </summary>
    #endregion
    IEnumerator UseItem<T>(T xValue) where T : IInteraction;

    #region 코루틴 설명
    /// <summary>
    /// 플레이어가 해당 아이템을 들고 있을경우의 행동.
    /// </summary>
    #endregion
    IEnumerator CarryItem(ItemSlot itemSlot);

    #region 코루틴 설명
    /// <summary>
    /// 플레이어가 이 아이템을 장착했을때의 행동입니다.
    /// </summary>
    #endregion
    IEnumerator MountItem();

    #region 코루틴 설명
    /// <summary>
    /// 플레이어가 이 아이템의 장착을 해제했을때의 행동입니다.
    /// </summary>
    #endregion
    IEnumerator UnmountItem();

    #region 코루틴 설명
    /// <summary>
    /// 해당 아이템이 인벤토리에 존재할 경우의 행동.
    /// </summary>
    #endregion
    IEnumerator InSlotItem();

    bool HasFunction(ItemInterface func);
}

public interface IUseItem
{
    void UseItem(InteractableObject target);
}
public interface IEquipItem
{
    void OnEquipItem();
    void DisEquipItem();
}

public class Item : MonoBehaviour
{
    [SerializeField] private ItemName _Name;
    [SerializeField] private Sprite _Sprite;

    [Space()]
    [SerializeField] private ItemCrystal _ItemCrystal;

    public Sprite Sprite => _Sprite;
    public ItemName Name => _Name;

    public float this[ItemElement element] => _ItemCrystal[element];

    [System.Obsolete]
    public    int  ItemCode
    {
        get { return _itemCode; }
    }
    protected int _itemCode;

    public    ItemTypeList  ItemType
    {
        get { return _itemType; }
    }
    protected ItemTypeList _itemType = ItemTypeList.NONE;
}
