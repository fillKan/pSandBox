using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeOak : Tree, Interaction
{
    public GameObject InteractObject()
    {
        return gameObject;
    }
    public void OperateAction<T>(T xValue) where T : ItemFunction
    {
        if (xValue == null) return;

        StartCoroutine(xValue.UseItem(this));
    }

    public void RegisterInteraction()
    {
        PlayerGetter.Instance.AddInteractObj(gameObject.GetInstanceID(), this);
    }


    public override void DropItem()
    {
        int repeat = Random.Range(8, 14);

        ItemExisting tItem;

        for (int i = 0; i < repeat; i++)
        {
            if (Random.Range(0, 4) == 0)
            {
                 tItem = ItemMaster.Instance.TakeItemExisting(ItemMaster.ItemList.SEED_OAK);
            }
            else tItem = ItemMaster.Instance.TakeItemExisting(ItemMaster.ItemList.LOG_OAK);

            tItem.gameObject.SetActive(true);
            tItem.transform.position = transform.position;
        }
    }

    protected override void InitTree()
    {
        _fDurability = 20;

        RegisterInteraction();

        TryGetComponent<SpriteRenderer>(out _sprtRenderer);
    }
}
