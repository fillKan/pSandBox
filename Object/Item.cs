using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 인터페이스 설명
/// <summary>
/// 아이템들의 기능하는 동작 코루틴들을 담는 인터페이스.
/// <para>※ 기능을 하는 아이템 클래스는 해당 인터페이스를 사용해야함. ※</para> 
/// </summary>
#endregion
public interface ItemFunction
{
    #region 코루틴 설명
    /// <summary>
    /// 플레이어가 해당 아이템을 들고, 오브젝트와 상호작용 했을 경우의 행동.
    /// </summary>
    #endregion
    IEnumerator UseItem<T>(T xValue) where T : Interaction;

    #region 코루틴 설명
    /// <summary>
    /// 플레이어가 해당 아이템을 들고 있을경우의 행동.
    /// </summary>
    #endregion
    IEnumerator CarryItem();

    #region 코루틴 설명
    /// <summary>
    /// 플레이어가 이 아이템을 장비하고 있을때에 행동.
    /// </summary>
    #endregion
    IEnumerator EquipItem();

    #region 코루틴 설명
    /// <summary>
    /// 해당 아이템이 인벤토리에 존재할 경우의 행동.
    /// </summary>
    #endregion
    IEnumerator InSlotItem();
}

public abstract class Item : MonoBehaviour
{
    [HideInInspector]
    public    int  ItemCode
    {
        get { return _itemCode; }
    }
    protected int _itemCode;

    public ItemMaster.ItemList ItemData
    {
        get { return (ItemMaster.ItemList)_itemCode; }
    }

    #region 변수 설명
    /// <summary>
    /// 인터페이스 ItemFunction의 UseItem코루틴의 실행 여부를 반환하는 변수.
    /// <para>
    /// ※ 인터페이스 ItemFunction가 구현된 객체에서만 해당 값이 유효함 ※
    /// </para>
    /// </summary>
    #endregion
    public    bool  IsUseItem
    {
        get { return _isUseItem; }
    }
    protected bool _isUseItem    = false;
    #region 변수 설명
    /// <summary>
    /// 인터페이스 ItemFunction의 CarryItem코루틴의 실행 여부를 반환하는 변수.
    /// <para>
    /// ※ 인터페이스 ItemFunction가 구현된 객체에서만 해당 값이 유효함 ※
    /// </para>
    /// </summary>
    #endregion 
    public    bool  IsCarryItem
    {
        get { return _isCarryItem; }
    }
    protected bool _isCarryItem  = false;

    #region 변수 설명
    /// <summary>
    /// 인터페이스 ItemFunction의 EquipItem코루틴의 실행 여부를 반환하는 변수.
    /// <para>
    /// ※ 인터페이스 ItemFunction가 구현된 객체에서만 해당 값이 유효함 ※
    /// </para>
    /// </summary>
    #endregion
    public    bool  IsEquipItem
    {
        get { return _isEquipItem; }
    }
    protected bool _isEquipItem  = false;

    #region 변수 설명
    /// <summary>
    /// 인터페이스 ItemFunction의 InSlotItem코루틴의 실행 여부를 반환하는 변수.
    /// <para>
    /// ※ 인터페이스 ItemFunction가 구현된 객체에서만 해당 값이 유효함 ※
    /// </para>
    /// </summary>
    #endregion
    public    bool  IsInSlotItem
    {
        get { return _isInSlotItem; }
    }
    protected bool _isInSlotItem = false;

    protected abstract void Init();

    private void Awake()
    {
        Init();

        TryGetComponent<Renderer>(out Renderer renderer);
                                               renderer.enabled = false;

        ItemMaster.Instance.Registration(this);
    }

}
