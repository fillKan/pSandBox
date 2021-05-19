using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DroppedItem : InteractableObject
{
    public Rigidbody2D Rigidbody => _Rigidbody;

    [Header("Dropped Property")]
    public ItemName Name;
    [SerializeField]
    private Rigidbody2D _Rigidbody;

    public override void Interaction()
    {
        PlayerGetter.Instance.Inventory.AddItem(this);
    }
}
