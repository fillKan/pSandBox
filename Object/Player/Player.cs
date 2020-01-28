using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 vDir;
    private SpriteRenderer sprite;

    private void OnEnable()
    {
        vDir = transform.position;

        sprite = gameObject.GetComponent<SpriteRenderer>();

        StartCoroutine(CR_update());
    }

    private IEnumerator CR_update()
    {
        while (gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space)) yield return StartCoroutine(CR_Vibration(0.1f, 0.3f));

            if (Input.GetAxis("Horizontal") != 0)
            {
                vDir.x += Input.GetAxis("Horizontal") * Time.deltaTime * 3.5f;

                if (Input.GetAxis("Horizontal") > 0) sprite.flipX = false;
                if (Input.GetAxis("Horizontal") < 0) sprite.flipX = true;

                transform.position = vDir;
            }

            yield return new WaitForFixedUpdate();
        }

        yield break;
    }

    private IEnumerator CR_Vibration(float amount, float time)
    {
        Vector2 vInitPos = transform.position;

        while(time > 0)
        {
            time -= Time.deltaTime;

            transform.position = ((Vector2)Random.insideUnitSphere * amount) + vInitPos;

            yield return new WaitForFixedUpdate();
        }
        transform.position = vInitPos;

        yield break;
    }
}
