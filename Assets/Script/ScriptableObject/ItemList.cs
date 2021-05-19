using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemList_", menuName = "Scriptable Object/ItemList")]
public class ItemList : ScriptableObject
{
    [SerializeField] private List<Item> _ItemList;

    public List<ItemName> GetList()
    {
        var list = new List<ItemName>();
        for (int i = 0;  i < _ItemList.Count; i++)
        {
            list.Add(_ItemList[i].Name);
        }
        return list;
    }
    public Dictionary<ItemName, Item> GetKeyValuePairs()
    {
        var pairs = new Dictionary<ItemName, Item>();

        foreach (var item in _ItemList)
        { pairs.Add(item.Name, item); }

        return pairs;
    }
}
