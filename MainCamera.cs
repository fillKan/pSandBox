using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Player player;
    private void Awake()
    {
        StartCoroutine(CR_tracePlayer());
    }

    private IEnumerator CR_tracePlayer()
    {
        while(player.enabled)
        {
            yield return null;

            transform.position = Vector3.Lerp(transform.position, player.transform.position, 0.07f);

            if (transform.position.y < -3.1f)
            {
                transform.Translate(0, -(transform.position.y + 3.1f), 0);
            }
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);

        }

        yield break;
    }
}
