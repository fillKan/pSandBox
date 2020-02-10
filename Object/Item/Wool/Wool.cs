using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wool : Item, Interaction
{
    public GameObject InteractObject()
    {
        return gameObject;
    }

    public void OperateAction()
    {
        gameObject.SetActive(false);
    }

    public void RegisterInteraction()
    {
        PlayerGetter.Instance.AddInteractObj(gameObject.GetInstanceID(), this);
    }

    protected override void Init()
    {
        RegisterInteraction();
        _itemCode = (int)ItemMaster.ItemList.WOOL;
    }
}
