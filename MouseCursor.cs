using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : Singleton<MouseCursor>
{
    private SpriteRenderer targetSprite;
    private Stack<Item> _carryItems = new Stack<Item>();
    private ItemSlot SelectSlot;
    public Stack<Item> CarryItems { get { return _carryItems; } }
    public Item CarryItem {
        get 
        {
            if (_carryItems.Count == 0) return null;
            return _carryItems.Peek(); 
        } 
    }
    public Camera MainCamera;

    public void AddCarryItem(Item item)
    {
        if(_carryItems.Count == 0)
        {
            _carryItems.Push(item);
            return;
        }
        if(_carryItems.Peek().itemCode == item.itemCode)
        {
            _carryItems.Push(item);
        }
    }

    private void Start()
    {
        StartCoroutine(CR_update());
    }

    private IEnumerator CR_update()
    {
        while(true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(SelectSlot != null)
                {
                    SelectSlot.OperateAction();
                }
                else if(CarryItem != null)
                {
                    _carryItems.Peek().transform.position = (Vector2)transform.position;
                    _carryItems.Peek().gameObject.SetActive(true);
                    _carryItems.Pop();
                }

                if (targetSprite != null)
                {
                    PlayerGetter.Instance.InteractCommend(targetSprite.gameObject.GetInstanceID());
                }
            }
            transform.position = MainCamera.ScreenToWorldPoint(Input.mousePosition);

            yield return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<ItemSlot>(out ItemSlot slot))
        {
            if (MouseRepeater.Instance.ActionObj.ContainsKey(slot.gameObject.GetInstanceID()))
            {
                SelectSlot = slot;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (PlayerGetter.Instance.GetInteractObj().ContainsKey(other.gameObject.GetInstanceID()))
        {
            SpriteRenderer spr = other.GetComponent<SpriteRenderer>();
            if(spr.Equals(targetSprite)) return;

            EnterObject(spr);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.TryGetComponent<ItemSlot>(out ItemSlot slot))
        {
            if (SelectSlot.Equals(slot)) SelectSlot = null;
        }

        if (targetSprite == null) return;

        if (other.gameObject.Equals(targetSprite.gameObject))
        {
            ExitObject();
        }
    }

    private void EnterObject(SpriteRenderer enterSpr)
    {
        if (targetSprite == null) targetSprite = enterSpr;

        else if(SortingLayer.GetLayerValueFromID(enterSpr.sortingLayerID) >= SortingLayer.GetLayerValueFromID(targetSprite.sortingLayerID))
        {
            targetSprite.color = Color.white;

            targetSprite = enterSpr;
        }
        if (enterSpr != targetSprite) return;

        targetSprite.color = new Color(0.9f, 1, 0.5f);
    }

    private void ExitObject()
    {
        targetSprite.color = Color.white;

        targetSprite = null;
    }
}
