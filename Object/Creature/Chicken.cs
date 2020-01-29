using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    private float fTimer = 0;
    private SpriteRenderer sprite;

    private void OnEnable()
    {
        sprite = GetComponent<SpriteRenderer>();

        StartCoroutine(CR_update());
    }

    private IEnumerator CR_update()
    {
        while(gameObject.activeSelf)
        {
            fTimer += Time.deltaTime;

            if(fTimer >= 1.5f)
            {
                fTimer = 0;

                if(Random.Range(0,3) != 2)
                {
                    yield return StartCoroutine(CR_movement());
                }
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    private IEnumerator CR_movement()
    {
        Debug.Log("Move!");

        Vector2 vDir;
        vDir.x = transform.position.x;
        vDir.y = transform.position.y;

        vDir.x = Random.Range(-3.0f, 3.1f);
        Debug.Log(vDir);
        Debug.Log(vDir.y);

        if (vDir.x == 0) yield break;

        else if(transform.position.x > vDir.x)
        {
            sprite.flipX = false;

            while (transform.position.x > vDir.x)
            {
                transform.position += (Vector3)vDir * Time.deltaTime;

                yield return new WaitForFixedUpdate();
            }
            yield break;
        }

        else if (transform.position.x < vDir.x)
        {
            sprite.flipX = true;

            while (transform.position.x < vDir.x)
            {
                transform.position += (Vector3)vDir * Time.deltaTime;

                yield return new WaitForFixedUpdate();
            }
            yield break;
        }
    }
}
