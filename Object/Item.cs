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
    IEnumerator CarryItem(ItemSlot itemSlot);

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

    #region 변수 설명
    /// <summary>
    /// 해당 아이템의 아이템 유형을 반환하는 변수. 기본값 : NONE
    /// </summary>
    #endregion
    public    ItemMaster.ItemType  ItemType
    {
        get { return _itemType; }
    }
    protected ItemMaster.ItemType _itemType = ItemMaster.ItemType.NONE;

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

    #region 함수 설명 : 
    /// <summary>
    /// 인터페이스 ItemFunction의 특정 기능의 실행 여부의 값을 조정하여, 조정에 성공했는지의 여부를 반환하는 함수.
    /// <para>
    /// 조정에 성공했다면, 인자값과 대치되는 기능이 실행중이 아니었다는 것 입니다. 이러한 의도로 함수를 사용하고, 이러한 의도의 함수라고 생각해주시길 바랍니다.
    /// </para>
    /// <para> 
    /// ※ 인터페이스 ItemFunction이 구현된 객체에서만 해당 함수의 사용이 유효하며, 해당 함수가 사용된 특정 기능이 종료될 때에는 StopWorking함수를 실행할 것을 권장함. ※
    /// </para>
    /// </summary>
    /// <param name="function">
    /// 실행여부를 판단할 값
    /// </param>
    /// <returns>
    /// 실행여부를 판단할 값이 조정에 성공했는지의 여부
    /// </returns>
    #endregion
    protected bool StartWorking(ref bool function)
    {
        if (function)
        {
            return false;
        }
        function = !function;

        return true;
    }
    #region 함수 설명 : 
    /// <summary>
    /// 인터페이스 ItemFunction의 특정 기능의 실행 여부를 '종료'로 조정하는 함수.
    /// <para> 
    /// ※ 인터페이스 ItemFunction이 구현된 객체에서만 해당 함수의 사용이 유효하며, 해당 함수가 사용되기 이전에 StartWorking함수를 실행할 것을 권장함. ※
    /// </para>
    /// </summary>
    /// <param name="function">
    /// 실행여부를 '종료'로 조정할 값
    /// </param>
    #endregion
    protected void  StopWorking(ref bool function)
    {       
        function = false;
    }

    protected abstract void Init();

    private void Awake()
    {
        Init();

        TryGetComponent<Renderer>(out Renderer renderer);
                                               renderer.enabled = false;

        ItemMaster.Instance.Registration(this);
    }

}
