using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [HideInInspector]
    public    int  ItemCode
    {
        get { return _itemCode; }
    }
    protected int _itemCode;

    protected abstract void Init();

    private void Awake()
    {
        Init();

        TryGetComponent<Renderer>(out Renderer renderer);
                                               renderer.enabled = false;

        //ItemMaster.Instance.Registration(this);
    }
}
