using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRadar : MonoBehaviour
{
    private Dictionary<int, DroppedItem> senseItem = new Dictionary<int, DroppedItem>();

    [Tooltip("CircleCollider의 Radius값을 지정합니다.")]
    public float Radius;

    #region 함수 설명 :
    /// <summary>
    /// 해당 레이더와 가장 가까이 있는 아이템을 반환합니다.
    /// </summary>
    #endregion
    public DroppedItem GetCloseItem()
    {
        DroppedItem closestItem = null;

        float distance = 0; float closestDistance = Radius;

        foreach(KeyValuePair<int,DroppedItem> item in senseItem)
        {
            distance = Vector2.Distance(transform.position, item.Value.transform.position);

            if(distance < closestDistance)
            {
                closestDistance = distance;

                closestItem = item.Value;
            }
        }
        return closestItem;
    }

    #region 함수 설명 :
    /// <summary>
    /// 해당 레이더와 가장 가까이 있는 아이템 오브젝트의 인스턴스 아이디를 반환합니다.
    /// </summary>
    #endregion
    public int GetCloseItemID()
    {
        DroppedItem closestItem = null;

        float distance = 0; float closestDistance = Radius;

        foreach (KeyValuePair<int, DroppedItem> item in senseItem)
        {
            distance = Vector2.Distance(transform.position, item.Value.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;

                closestItem = item.Value;
            }
        }
        return closestItem.gameObject.GetInstanceID();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out DroppedItem item))
        {
            if(!senseItem.ContainsKey(item.gameObject.GetInstanceID()))
            {
                senseItem.Add(item.gameObject.GetInstanceID(), item);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(senseItem.ContainsKey(collision.gameObject.GetInstanceID()))
        {
            senseItem.Remove(collision.gameObject.GetInstanceID());
        }
    }
}
