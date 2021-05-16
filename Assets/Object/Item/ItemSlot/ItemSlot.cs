using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerDownHandler
{
    private const string Zero = "0";
    private const float ItemSpriteScaling = 0.8f;

    public event Action<ItemName> ExitItem;
    public event Action<ItemName> EnterItem;

    [SerializeField] private Image _ItemRenderer;
    [SerializeField] private TMPro.TextMeshProUGUI _CountText;
    [SerializeField] private Sprite _EmptySprite;

    [Space(10f)]
    [SerializeField] private ItemName _ContainItem;
    [SerializeField] private int _ItemCount;

    public ItemName ContainItem
    { get => _ContainItem; }
    public int ItemCount
    { get => _ItemCount; }

    public void OnPointerDown(PointerEventData eventData)
    {
        ItemName carrying = CursorPointer.Instance.CarryingItem;

        if (Input.GetMouseButtonDown(0))
        {
            if (carrying != ItemName.NONE)
            {
                if (carrying == ContainItem)
                {
                    AddItem();
                    CursorPointer.Instance.SubtractCarryingItem();
                }
                else if (ContainItem == ItemName.NONE)
                {
                    AddItem(carrying);
                    CursorPointer.Instance.SubtractCarryingItem();
                }
            }
        }
        else if (Input.GetMouseButtonDown(1) && _ItemCount != 0)
        {
            if (carrying == ItemName.NONE || carrying == ContainItem)
            {
                CursorPointer.Instance.AddCarryingItem(ContainItem);
                SubtractItem();
            }
        }
    }

    public void AddItem(ItemName itemName, int count = 1)
    {
        if (itemName == ItemName.NONE) return;

        if (itemName == ContainItem)
        {
            _ItemCount += count;
            TextUpdate();
        }
        else if (_ContainItem == ItemName.NONE)
        {
            ExitItem?.Invoke(itemName);

            _ContainItem = itemName;
            
            var sprite = ItemMaster.Instance.GetItemSprt(itemName);

            _ItemRenderer.sprite = sprite;
            _ItemRenderer.rectTransform.sizeDelta 
                = new Vector2(sprite.rect.width, sprite.rect.height) * ItemSpriteScaling;

            _ItemCount += count;

            TextUpdate();
        }
    }
    public void AddItem(int count = 1)
    {
        _ItemCount += count;
        TextUpdate();
    }
    public void SubtractItem(int count = 1)
    {
        if ((_ItemCount -= count) <= 0)
        {
            ExitItem?.Invoke(ContainItem);

            _CountText.text = Zero;
            _ItemRenderer.sprite = _EmptySprite;

            _ContainItem = ItemName.NONE;
            _ItemCount = 0;
        }
        else TextUpdate();
    }

    public void TextUpdate()
    {
        _CountText.text = _ItemCount.ToString();
    }
}