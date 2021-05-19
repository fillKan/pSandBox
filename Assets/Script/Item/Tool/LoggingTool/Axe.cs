using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Item, IEquipItem
{
    [SerializeField] private float LoggingValue;

    public void DisEquipItem()
    {
        PlayerStat.Instance[Stat.Logging] -= LoggingValue;
    }
    public void OnEquipItem()
    {
        PlayerStat.Instance[Stat.Logging] += LoggingValue;
    }
    public override bool IsUsing(ItemInterface itemInterface)
    {
        return itemInterface == ItemInterface.Equip;
    }
}
