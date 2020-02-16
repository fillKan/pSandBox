using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakMotion : MonoBehaviour
{
    public float fMaxSpeed = 0;
    public Rigidbody2D Rigidbody;

    public BreakMotion(Rigidbody2D rigidbody, float maxSpeed)
    {
        Rigidbody = rigidbody;
        fMaxSpeed = maxSpeed;
    }

    public void ChkOperCondition()
    {
        if(Rigidbody.velocity.y == 0)
        {
            return;
        }

        if(Rigidbody.velocity.y <= fMaxSpeed)
        {
            Rigidbody.bodyType = RigidbodyType2D.Kinematic;
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 0);
        }
    }
}
