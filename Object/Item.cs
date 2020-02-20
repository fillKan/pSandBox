using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [HideInInspector]
    public    int  itemCode
    {
        get { return _itemCode; }
    }
    protected int _itemCode;

    public    Rigidbody2D  Rigidbody
    {
        get { return _rigidbody; }
    }
    protected Rigidbody2D _rigidbody;

    protected abstract void Init();

    private void Awake()
    {
        Init();

        TryGetComponent<SpriteRenderer>(out SpriteRenderer sprite);

        ItemMaster.Instance.Registration(this, sprite.sprite);
    }
}
