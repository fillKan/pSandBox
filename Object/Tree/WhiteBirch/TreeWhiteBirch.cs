using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeWhiteBirch : Tree
{
    private Stack<GameObject> dropItems = new Stack<GameObject>();

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
        GameObject tItem;
        
        for(int i = 0; i < repeat; i++)
        {
            if(Random.Range(0,2) == 0)
            {
                 tItem = Instantiate(ItemPool.Instance.GetItem(ItemPool.ItemList.LOG_WHITEBIRCH), transform.position, Quaternion.identity);             
            }
            else tItem = Instantiate(ItemPool.Instance.GetItem(ItemPool.ItemList.LOG_WHITEBIRCH), transform.position, Quaternion.identity);

            tItem.SetActive(false);

            dropItems.Push(tItem);
        }

        sprite = gameObject.GetComponent<SpriteRenderer>();

        rect.SetRect(1, -1, -2, -5);
    }
}
