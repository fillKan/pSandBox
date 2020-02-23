using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 클래스 설명 : 
/// <summary>
/// 땅에 떨어져있는 아이템의 스프라이트를 띄우는 클래스.
/// <para>
/// ※ Rigidbody2D와 BoxCollider2D컴포넌트를 포함해야 합니다. ※
/// </para>
/// </summary>
#endregion
public class ItemSprt : MonoBehaviour, Interaction
{
    public ItemMaster.ItemList ItemCode;

    public GameObject InteractObject()
    {
        return gameObject;
    }

    public void OperateAction()
    {
        PlayerGetter.Instance.Inventory.AddItemInventory(this);
    }

    public void RegisterInteraction()
    {
        PlayerGetter.Instance.AddInteractObj(gameObject.GetInstanceID(), this);
    }
}
