using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [HideInInspector]
    public int itemCode
    {
        get { return _itemCode; }
    }
    protected int _itemCode;

    protected abstract void Init();

    private void Reset()
    {
        Init();
    }
}
