using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    protected static readonly Color _HighLightColor = new Color(0.9f, 1.0f, 0.5f);

    [Header("Intractable Property")]
    [SerializeField]
    protected SpriteRenderer _Renderer;
    public SpriteRenderer Renderer
    { get => _Renderer; }

    public abstract void Interaction();
    public virtual void OnHighLight()
    {
        _Renderer.color = _HighLightColor;
    }
}
