using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemSlot[]   ItemSlots;
    public ItemSlot EquipItemSlot;

    [SerializeField, Space()]
    private Transform _EquipHandSlot;

    private Item _EquipedHandItem;
    private IEquipItem _EquipedHandInterface;

    private void Start()
    {
        EquipItemSlot.EnterItem += item =>
        {
            if (item == default) return;

            _EquipedHandItem = ItemMaster.Instance.GetItemObject(item);
            
            _EquipedHandItem.transform.SetParent(_EquipHandSlot);
            _EquipedHandItem.transform.localPosition = Vector3.zero;
            _EquipedHandItem.transform.localScale = Vector3.one;

            if (_EquipedHandItem.IsUsing(ItemInterface.Equip))
            {
                if (_EquipedHandItem.TryGetComponent(out _EquipedHandInterface)) {
                    _EquipedHandInterface.OnEquipItem();
                }
            }
        };
        EquipItemSlot.ExitItem += item =>
        {
            if (_EquipedHandItem == null) return;

            _EquipedHandItem.transform.SetParent(null);
            ItemMaster.Instance.AddItemObject(_EquipedHandItem);

            _EquipedHandItem = null;

            _EquipedHandInterface?.DisEquipItem();
            _EquipedHandInterface = null;
        };
    }
    public void AddItem(DroppedItem item)
    {
        var itemName = item.Name;
        int emptySlotIndex = -1;

        for (int i = 0; i < ItemSlots.Length; i++)
        {
            if (ItemSlots[i].ContainItem == itemName) 
            {
                ItemSlots[i].AddItem();
                ItemMaster.Instance.AddDroppedItem(item);
                return;
            }
            else if (emptySlotIndex == -1 && ItemSlots[i].ContainItem == default)
            {
                emptySlotIndex = i;
            }
        }
        if (emptySlotIndex != -1)
        {
            ItemSlots[emptySlotIndex].AddItem(itemName);
            ItemMaster.Instance.AddDroppedItem(item);
        }
    }
}
