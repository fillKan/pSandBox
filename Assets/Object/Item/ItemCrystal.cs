using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemElement
{
    None, Fuel, Wood, Fish, CottonFabric, CottonWool, Tool, Food
}
[Serializable] public struct ItemElementValuePair
{
    [Space()]
    public ItemElement Element;
    public float Value;
}
public class ItemCrystal : MonoBehaviour
{
    [SerializeField] private ItemElementValuePair[] _CrystalElements;
    private ItemElementValuePair _Cache;

    public float this[ItemElement element]
    {
        get
        {
            if (_Cache.Element == element)
            {
                return _Cache.Value;
            }
            for (int i = 0; i < _CrystalElements.Length; i++)
            {
                if (_CrystalElements[i].Element == element)
                {
                    _Cache = _CrystalElements[i];
                    return _CrystalElements[i].Value;
                }
            }
            return 0;
        }
    }
}
