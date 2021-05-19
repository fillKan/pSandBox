using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    protected static readonly Color _HighLightColor = new Color(0.9f, 1.0f, 0.5f);
    [Header("Intractable Property")]
    [SerializeField] protected SpriteRenderer _Renderer;
    [SerializeField] protected Color _DefaultColor = Color.white;

    public bool IsHighLighted
    { get; protected set; }
    public SpriteRenderer Renderer
    { get => _Renderer; }

    public abstract void Interaction();
    public virtual void OnHighLight()
    {
        _Renderer.color = _HighLightColor;
        IsHighLighted = true;
    }
    public virtual void DisHighLight()
    {
        _Renderer.color = _DefaultColor;
        IsHighLighted = false;
    }
    public virtual void OnActive() { }
    private void OnEnable()
    {
        InteractionManager.Instance.Register(this);

        OnActive();
    }
}
