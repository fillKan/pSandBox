using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeWhiteBirch : Tree
{
    protected override void DropItem()
    {
       
    }

    protected override void InitTree()
    {
        fDurability = 20;

        sprite = gameObject.GetComponent<SpriteRenderer>();
    }
}
