﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oak_seed : Item, Interaction
{
    public GameObject InteractObject()
    {
        return gameObject;
    }

    public void OperateAction()
    {
        //PlayerGetter.Instance.Inventory.AddItemInventory(this);
    }

    public void RegisterInteraction()
    {
        PlayerGetter.Instance.AddInteractObj(gameObject.GetInstanceID(), this);
    }
    protected override void Init()
    {
        RegisterInteraction();

        _itemCode = (int)ItemMaster.ItemList.SEED_OAK;
    }
}
