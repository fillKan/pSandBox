using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRadar : MonoBehaviour
{
    private Dictionary<int, ItemExisting> senseItem = new Dictionary<int, ItemExisting>();

    #region 함수 설명 :
    /// <summary>
    /// 해당 레이더와 가장 가까이 있는 아이템을 반환합니다.
    /// </summary>
    #endregion
    public ItemExisting GetCloseItem()
    {
        ItemExisting closestItem = null;

        float distance = 0; float closestDistance = 0;

        foreach(KeyValuePair<int,ItemExisting> item in senseItem)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out ItemExisting item))
        {
            if(senseItem.ContainsKey(item.gameObject.GetInstanceID()))
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
