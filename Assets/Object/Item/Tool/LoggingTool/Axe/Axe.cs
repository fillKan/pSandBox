using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Item, IEquipItem
{
    [SerializeField] private float LoggingValue;

    public void DisEquipItem()
    {
        StateStorage.Instance.DecreaseState(States.TREE_LOGGING, LoggingValue);
    }
    public void OnEquipItem()
    {
        StateStorage.Instance.IncreaseState(States.TREE_LOGGING, LoggingValue);
    }
    public override bool IsUsing(ItemInterface itemInterface)
    {
        return itemInterface == ItemInterface.Equip;
    }
}
