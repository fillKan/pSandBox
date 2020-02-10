using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : Singleton<MouseCursor>
{
    private Vector2 vPrevPos;
    private Vector2 vCurrPos;

    private SpriteRenderer targetSprite;
    public Camera MainCamera;

    private void Start()
    {
        StartCoroutine(CR_update());
    }

    private IEnumerator CR_update()
    {
        vCurrPos = Input.mousePosition;
        vPrevPos = vCurrPos;

        while(true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (targetSprite != null)
                {
                    PlayerGetter.Instance.InteractCommend(targetSprite.gameObject.GetInstanceID());
                }
            }

            vCurrPos = Input.mousePosition;

            transform.position = MainCamera.ScreenToWorldPoint(Input.mousePosition);

            yield return null;
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
        if (targetSprite == null) return;

        if (other.gameObject.Equals(targetSprite.gameObject))
        {
            ExitObject();
        }
    }

    private void EnterObject(SpriteRenderer enterSpr)
    {
        if (targetSprite == null) targetSprite = enterSpr;

        else if(enterSpr.sortingLayerID <= targetSprite.sortingLayerID)
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
