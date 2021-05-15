using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorPointer : Singleton<CursorPointer>
{
    [SerializeField] private SpriteRenderer _Renderer;

    // ===== CarryingItem ===== //

    public int CarryingCount
    { get; private set; }
    public ItemName CarryingItem
    { get; private set; }

    // ===== CarryingItem ===== //

    private Camera _MainCamera;
    public InteractableObject Highlighted
    { get; private set; }

    private void Awake()
    {
        _MainCamera = Camera.main;

        CarryingCount = 0;
        CarryingItem = ItemName.NONE;
    }
    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Highlighted != null)
            {
                PlayerController.Instance.Interaction(Highlighted);
            }
            else if (IsOnVoid())
            {
                PlayerController.Instance.MoveToPoint(transform.position);
            }
        }
        transform.position = _MainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.Translate(0, 0, 10.0f);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        InteractableObject interactable;
        if (InteractionManager.Instance.IsInteractable(collision.gameObject, out interactable))
        {
            if (Highlighted != null)
            {
                var hRenderer = Highlighted.Renderer;
                var cRenderer = interactable.Renderer;

                if (cRenderer.sortingLayerID > hRenderer.sortingLayerID)
                {
                    Highlighted.DisHighLight();
                    interactable.OnHighLight();

                    Highlighted = interactable;
                }
                else if (cRenderer.sortingLayerID == hRenderer.sortingLayerID)
                {
                    if (cRenderer.sortingOrder >= hRenderer.sortingOrder)
                    {
                        Highlighted.DisHighLight();
                        interactable.OnHighLight();

                        Highlighted = interactable;
                    }
                }
            }
            else
            {
                Highlighted = interactable;
                interactable.OnHighLight();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Highlighted == null) return;

        if (collision.gameObject.GetInstanceID() == Highlighted.gameObject.GetInstanceID())
        {
            Highlighted.DisHighLight();
            Highlighted = null;
        }
    }
    public bool IsOnVoid()
    {
        return !EventSystem.current.IsPointerOverGameObject() && Highlighted == null;
    }
    public void AddCarryingItem(ItemName item, int count = 1)
    {
        if (item == CarryingItem || CarryingItem == ItemName.NONE)
        {
            CarryingCount += count;

            if (CarryingItem == ItemName.NONE)
            {
                CarryingItem = item;
                _Renderer.sprite = ItemMaster.Instance.GetItemSprt(item);
            }
        }
    }
    public void SubtractCarryingItem(int count = 1)
    {
        if (CarryingCount == 0) return;

        CarryingCount -= count;

        if (CarryingCount <= 0)
        {
            CarryingCount = 0;
            CarryingItem = ItemName.NONE;

            _Renderer.sprite = null;
        }
    }
    public void DropCarryingItem()
    {
        if (CarryingCount > 0)
        {
            CarryingCount--;

            ItemMaster.Instance.GetDroppedItem(CarryingItem)
                .transform.position = transform.position;
        }
    }
}
