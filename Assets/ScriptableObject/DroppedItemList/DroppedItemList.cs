using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DroppedItemList_", menuName = "Scriptable Object/DroppedItemList")]
public class DroppedItemList : ScriptableObject
{
    [SerializeField] private List<DroppedItem> _DroppedItemInstances;

    public Dictionary<ItemName, DroppedItem> GetKeyValuePairs()
    {
        var pairs = new Dictionary<ItemName, DroppedItem>();

        foreach (var itemPair in _DroppedItemInstances)
        {
            pairs.Add(itemPair.ItemData, itemPair);
        }
        return pairs;
    }
}
