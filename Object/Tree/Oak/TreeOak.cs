using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeOak : Tree, Interaction
{
    private Stack<ItemExisting> TakeItemExistings = new Stack<ItemExisting>();

    public GameObject InteractObject()
    {
        return gameObject;
    }

    public void OperateAction()
    {
        if(!doingChopTree)
        {
            StartCoroutine(CR_chopTree());
        }
    }

    public void RegisterInteraction()
    {
        PlayerGetter.Instance.AddInteractObj(gameObject.GetInstanceID(), this);
    }

    protected override void DropItem()
    {
        while (TakeItemExistings.Count != 0)
        {
            TakeItemExistings.Peek().gameObject.SetActive(true);

            TakeItemExistings.Pop();
        }
    }

    protected override void InitTree()
    {
        fDurability = 20;
        int repeat = Random.Range(8, 14);
        ItemExisting tItem;

        RegisterInteraction();

        for (int i = 0; i < repeat; i++)
        {
            if (Random.Range(0, 4) == 0)
            {
                 tItem = Instantiate(ItemMaster.Instance.GetItemExisting(ItemMaster.ItemList.SEED_OAK), transform.position, Quaternion.identity);
            }
            else tItem = Instantiate(ItemMaster.Instance.GetItemExisting(ItemMaster.ItemList.LOG_OAK), transform.position, Quaternion.identity);

            tItem.gameObject.SetActive(false);

            TakeItemExistings.Push(tItem);
        }

        sprite = gameObject.GetComponent<SpriteRenderer>();

    }
}
