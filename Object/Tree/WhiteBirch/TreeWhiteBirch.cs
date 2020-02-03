using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeWhiteBirch : Tree, Interaction
{
    private Stack<Item> dropItems = new Stack<Item>();

    public void OperateAction()
    {
        if (!doingChopTree)
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
        while (dropItems.Count != 0)
        {
            dropItems.Peek().gameObject.SetActive(true);

            dropItems.Pop();
        }
    }

    protected override void InitTree()
    {
        fDurability = 20;
        int repeat = Random.Range(8, 14);
        Item tItem;

        RegisterInteraction();

        for (int i = 0; i < repeat; i++)
        {
            if(Random.Range(0,4) == 0)
            {
                 tItem = Instantiate(ItemMaster.Instance.GetItem(ItemMaster.ItemList.SEED_WHITEBIRCH), transform.position, Quaternion.identity);             
            }
            else tItem = Instantiate(ItemMaster.Instance.GetItem(ItemMaster.ItemList.LOG_WHITEBIRCH), transform.position, Quaternion.identity);

            tItem.gameObject.SetActive(false);

            dropItems.Push(tItem);
        }

        sprite = gameObject.GetComponent<SpriteRenderer>();

    }
}
