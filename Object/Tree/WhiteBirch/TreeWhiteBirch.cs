using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeWhiteBirch : Tree, Interaction
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
                 tItem = Instantiate(ItemMaster.Instance.GetItemExisting(ItemMaster.ItemList.SEED_WHITEBIRCH), transform.position, Quaternion.identity);
            }
            else tItem = Instantiate(ItemMaster.Instance.GetItemExisting(ItemMaster.ItemList.LOG_WHITEBIRCH), transform.position, Quaternion.identity);

            tItem.gameObject.SetActive(true);
        }
    }

    protected override void InitTree()
    {
        fDurability = 20;

        RegisterInteraction();

        TryGetComponent<SpriteRenderer>(out SprtRenderer);
    }
}
