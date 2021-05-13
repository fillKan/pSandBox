using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ItemSpriteSet
{
    public ItemName ItemName;
    public Sprite Sprite;
}
[CreateAssetMenu(fileName = "SpriteList_", menuName = "Scriptable Object/ItemSpriteList")]
public class ItemSpriteList : ScriptableObject
{
    [SerializeField] private List<ItemSpriteSet> SpriteList;

    public Dictionary<ItemName, Sprite> GetKeyValuePairs()
    {
        var pairs = new Dictionary<ItemName, Sprite>();

        foreach (var spriteSet in SpriteList)
        {
            pairs.Add(spriteSet.ItemName, spriteSet.Sprite);
        }
        return pairs;
    }
}
