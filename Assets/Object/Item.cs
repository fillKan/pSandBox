using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemInterface
{
    Use, Equip
}
public interface IUseItem
{
    void UseItem(InteractableObject target);
}
public interface IEquipItem
{
    void OnEquipItem();
    void DisEquipItem();
}

public class Item : MonoBehaviour
{
    [SerializeField] private ItemName _Name;
    [SerializeField] private Sprite _Sprite;

    [Space()]
    [SerializeField] private ItemCrystal _ItemCrystal;

    public Sprite Sprite => _Sprite;
    public ItemName Name => _Name;

    public float this[ItemElement element] => _ItemCrystal[element];

    public virtual bool IsUsing(ItemInterface itemInterface)
    {
        return false;
    }
}
