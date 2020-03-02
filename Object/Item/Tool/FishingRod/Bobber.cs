using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    public Rigidbody2D GetRigidbody2D;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Water"))
        {
            StartCoroutine(CR_waitBiting());
        }
    }

    private IEnumerator CR_waitBiting()
    {
        float waitTime = Random.Range(5.0f, 10.0f);

        for(float i = 0; i < waitTime; i += Time.deltaTime)
        {
            yield return null;
        }

        Vector2 InitPos = transform.position;

        for (float i = 0; i < 0.6f; i += Time.deltaTime)
        {
            transform.position = InitPos + (Random.insideUnitCircle * 0.05f);

            yield return null;
        }
        transform.position = InitPos;

        yield break;
    }
}
