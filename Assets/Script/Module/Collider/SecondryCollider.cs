using System;
using UnityEngine;

public class SecondryCollider : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _Collider;
    public BoxCollider2D Collider => _Collider;

    public event Action<Collider2D> OnTriggerEnter;
    public event Action<Collider2D> OnTriggerStay;
    public event Action<Collider2D> OnTriggerExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter?.Invoke(collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerStay?.Invoke(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerExit?.Invoke(collision);
    }
}
