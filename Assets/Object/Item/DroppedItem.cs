using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : InteractableObject
{
    public Rigidbody2D Rigidbody => _Rigidbody;

    [Header("Dropped Property")]
    public ItemName ItemData;
    [SerializeField]
    private Rigidbody2D _Rigidbody;

    public override void Interaction()
    {
        PlayerGetter.Instance.Inventory.AddItemInventory(this);
    }
}
