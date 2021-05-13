using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct ItemSpriteSet
{
    public ItemName ItemName;
    public Sprite Sprite;
}
[CreateAssetMenu(fileName = "SpriteList_", menuName = "Scriptable Object/ItemSpriteList")]
public class ItemSpriteList : ScriptableObject
{
    [SerializeField] private List<ItemSpriteSet> SpriteList;

    public Sprite this[ItemName itemName]
    {
        get => SpriteList[(int)itemName].Sprite;
    }
}
