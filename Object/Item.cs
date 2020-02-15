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

    private void Awake()
    {
        Init();

        ItemMaster.Instance.Registration(this);
    }

    public void ContainItem(Transform parent)
    {
        transform.parent = parent;
        transform.localPosition = Vector2.zero;
        transform.localScale = new Vector3(0.7f, 0.7f, 1);

        gameObject.SetActive(true);
        TryGetComponent<SpriteRenderer>(out SpriteRenderer spr);
        TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody);
        TryGetComponent<BoxCollider2D>(out BoxCollider2D box);
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        
        box.enabled = false;
        spr.sortingLayerID = FindObjectOfType<ItemSlot>().GetComponent<SpriteRenderer>().sortingLayerID;
        spr.sortingOrder = 1;
    }
}
