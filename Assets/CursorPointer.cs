using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorPointer : Singleton<CursorPointer>
{
    private Camera _MainCamera;
    public InteractableObject Highlighted
    { get; private set; }

    private void Awake()
    {
        _MainCamera = Camera.main;
    }
    private void LateUpdate()
    {
        if (Highlighted != null) {
            if (Input.GetMouseButtonDown(0))
            {
                PlayerController.Instance.Interaction(Highlighted);
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
}
