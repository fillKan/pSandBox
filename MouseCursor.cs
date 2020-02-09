using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : Singleton<MouseCursor>
{
    private Vector2 vPrevPos;
    private Vector2 vCurrPos;

    public Camera MainCamera;

    private void Start()
    {
        StartCoroutine(CR_update());
    }

    private IEnumerator CR_update()
    {
        int layer = 1 << LayerMask.NameToLayer("Player Interaction");

        vPrevPos.x = (long)(Input.mousePosition.x);
        vPrevPos.y = (long)(Input.mousePosition.y);
        vCurrPos = vPrevPos;

        while(true)
        {
            vCurrPos.x = (long)(Input.mousePosition.x);
            vCurrPos.y = (long)(Input.mousePosition.y);

            if (vCurrPos != vPrevPos)
            {
                vPrevPos = vCurrPos;
                Vector2 vRayPos = MainCamera.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D hit2D = Physics2D.Raycast(vRayPos, Vector2.zero, 10, layer);

                if (hit2D)
                {
                    Debug.Log(hit2D.point);
                }
            }
            yield return null;
        }
    }
}
