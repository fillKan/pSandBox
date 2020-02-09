using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : Singleton<MouseCursor>
{
    private Vector2 vPrevPos;
    private Vector2 vCurrPos;

    private bool castRay = false;

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
            vCurrPos = Input.mousePosition;

            transform.position = MainCamera.ScreenToWorldPoint(Input.mousePosition);

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

    }
}
