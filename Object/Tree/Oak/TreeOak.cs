using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeOak : Tree
{
    protected override void DropItem()
    {

    }

    protected override void InitTree()
    {
        fDurability = 20;

        sprite = gameObject.GetComponent<SpriteRenderer>();

        rect.SetRect(1, -1, -2, -5);
    }
}
