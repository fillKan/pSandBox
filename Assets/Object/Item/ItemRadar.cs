using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRadar : MonoBehaviour
{
    private LinkedList<DroppedItem> _DroppedItems = new LinkedList<DroppedItem>();

    public DroppedItem GetCloseItem()
    {
        if (_DroppedItems.Count == 0) return null;

        Vector2 pos = transform.position;

        float closeLength = float.MaxValue;
        DroppedItem cloes = null;

        for (var i = _DroppedItems.First; i != null; )
        {
            if (!i.Value.gameObject.activeSelf)
            {
                var next = i.Next;
                _DroppedItems.Remove(i);
                i = next;
                continue;
            }
            float length = Vector2.Distance(i.Value.transform.position, pos);

            if (length < closeLength)
            {
                closeLength = length;
                cloes = i.Value;
            }
            i = i.Next;
        }
        return cloes;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out DroppedItem dropped))
        {
            _DroppedItems.AddLast(dropped);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_DroppedItems.Count == 0) return;

        int compareID = collision.gameObject.GetInstanceID();
        for (var i = _DroppedItems.First; i != null; i = i.Next)
        {
            if (i.Value.gameObject.GetInstanceID() == compareID)
            {
                _DroppedItems.Remove(i);
            }
        }
    }
}
