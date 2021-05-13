using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : InteractableObject
{
    public int ItemCode
    {
        get { return (int)ItemData; }
    }

    [Header("Dropped Property")]
    public ItemList ItemData;

    public override void Interaction()
    {
        PlayerGetter.Instance.Inventory.AddItemInventory(this);
    }
}
