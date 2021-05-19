using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorPointer : Singleton<CursorPointer>
{
    private const float ImageRectScaling = 1.1f;
    private const float TextPositionScalingX = 0.21484375f;
    private const float TextPositionScalingY = 0.234375f;

    private readonly Vector3 Offset = new Vector2(-Screen.width, -Screen.height) / 2f;

    [SerializeField] private Image _Renderer;
    [SerializeField] private Transform _CarryingInformator;
    [SerializeField] private TMPro.TextMeshProUGUI _ItemCountText;

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
        else if (Input.GetAxis("Mouse ScrollWheel") > 0.0f || Input.GetMouseButtonDown(2))
        {
            DropCarryingItem();
        }
        transform.position = _MainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.Translate(0, 0, 10.0f);

        if (_CarryingInformator.gameObject.activeSelf) {
            _CarryingInformator.localPosition = Input.mousePosition + Offset;
        }
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
            HighLightRelease();
        }
    }
    public bool IsOnVoid()
    {
        return !EventSystem.current.IsPointerOverGameObject() && Highlighted == null;
    }
    public void HighLightRelease()
    {
        Highlighted?.DisHighLight();
        Highlighted = null;
    }
    public void AddCarryingItem(ItemName item, int count = 1)
    {
        if (item == CarryingItem || CarryingItem == ItemName.NONE)
        {
            CarryingCount += count;

            if (CarryingItem == ItemName.NONE)
            {
                CarryingItem = item;

                var sprite = ItemMaster.Instance.GetItemSprite(item);

                _Renderer.sprite = sprite;
                _Renderer.rectTransform.sizeDelta
                    = new Vector2(sprite.rect.width, sprite.rect.height) * ImageRectScaling;

                _ItemCountText.transform.localPosition 
                    = new Vector3(sprite.rect.width * TextPositionScalingX, sprite.rect.height * TextPositionScalingY);

                _CarryingInformator.gameObject.SetActive(true);
            }
            TextUpdate();
        }
    }
    public void SubtractCarryingItem(int count = 1)
    {
        if (CarryingCount > 0) {
            CarryingCount -= count;

            if (CarryingCount < 1) {

                CarryingCount = 0;
                CarryingItem = ItemName.NONE;

                _Renderer.sprite = null;
                _CarryingInformator.gameObject.SetActive(false);
            }
            TextUpdate();
        }
    }
    public void DropCarryingItem()
    {
        if (CarryingCount > 0)
        {
            ItemMaster.Instance.GetDroppedItem(CarryingItem)
                .transform.position = transform.position;

            SubtractCarryingItem();
            TextUpdate();
        }
    }
    public void TextUpdate()
    {
        _ItemCountText.text = CarryingCount.ToString();
    }
}
