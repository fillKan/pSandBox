using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wool : Item, Interaction
{
    private BreakMotion breakMotion;
    private Rigidbody2D Rigidbody2D;
    public GameObject InteractObject()
    {
        return gameObject;
    }

    public void OperateAction()
    {
        PlayerGetter.Instance.Inventory.AddItemInventory(this);
    }

    public void RegisterInteraction()
    {
        PlayerGetter.Instance.AddInteractObj(gameObject.GetInstanceID(), this);
    }

    protected override void Init()
    {
        RegisterInteraction();
        _itemCode = (int)ItemMaster.ItemList.WOOL;
        TryGetComponent<Rigidbody2D>(out Rigidbody2D);
        breakMotion = new BreakMotion(Rigidbody2D, -10);
    }

    private void Update()
    {
        breakMotion.ChkOperCondition();
    }
}
