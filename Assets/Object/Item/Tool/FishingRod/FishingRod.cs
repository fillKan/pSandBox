using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : Item, IEquipItem
{
    private const float MaxLineLength = 15f;

    [Header("FishingRod Property")]
    [SerializeField] private Sprite _UsedSprite;
    [SerializeField] private Sprite _DefaultSprite;
    [SerializeField] private SpriteRenderer _Renderer;

    [SerializeField] private LineRenderer _LineRenderer;
    [SerializeField] private Transform _RodTopPoint;

    [Header("Bobber Property")]
    [SerializeField] private SecondryCollider _Bobber;
    [SerializeField] private Rigidbody2D _BobberRigid;
    [SerializeField] private float _ThrowingForce;

    private CursorPointer _Cursor;

    private bool _IsEquiped;
    private bool _IsUsed = false;

    public void OnEquipItem()
    {
        _IsEquiped = true;
        StartCoroutine(UpdateRoutine());

        _Bobber.gameObject.SetActive(false);
        _LineRenderer.gameObject.SetActive(false);

        _Cursor ??= CursorPointer.Instance;
    }
    public void DisEquipItem()
    {
        _IsEquiped = false;
        DisThrowBobber();
    }
    private IEnumerator UpdateRoutine()
    {
        while (_IsEquiped)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (_Cursor.IsOnVoid())
                {
                    if (_IsUsed)
                    {
                        DisThrowBobber();
                    }
                    else
                    {
                        OnThrowBobber();
                    }
                    _IsUsed = !_IsUsed;
                }
            }
            Vector2 bobberPos = _Bobber.transform.position;
            Vector2 rodTopPos = _RodTopPoint.position;

            if (_IsUsed && Vector2.Distance(bobberPos, rodTopPos) > MaxLineLength)
            {
                BobberDisable();
            }
            else if (_Bobber.gameObject.activeSelf)
            {
                _LineRenderer.SetPosition(0, rodTopPos);
                _LineRenderer.SetPosition(1, bobberPos);
            }
            yield return null;
        }
    }
    private void OnThrowBobber()
    {
        _Renderer.sprite = _UsedSprite;

        _Bobber.transform.SetParent(null);
        _Bobber.gameObject.SetActive(true);
        _LineRenderer.gameObject.SetActive(true);

        Vector2 dir = (_Cursor.transform.position - _RodTopPoint.position);
        _BobberRigid.AddForce(dir.normalized * _ThrowingForce * dir.magnitude);
    }
    private void DisThrowBobber()
    {
        Vector2 dir = (_RodTopPoint.position - _Bobber.transform.position);
        Vector2 force = dir.normalized * _ThrowingForce * dir.magnitude;

        _BobberRigid.velocity = force;
        _BobberRigid.AddForce(force);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(_Bobber.gameObject) && !_IsUsed)
        {
            BobberDisable();
        }
    }
    private void BobberDisable()
    {
        _Renderer.sprite = _DefaultSprite;

        _Bobber.transform.SetParent(transform);
        _Bobber.gameObject.SetActive(false);
        _LineRenderer.gameObject.SetActive(false);

        _Bobber.transform.position = _RodTopPoint.position;
        _BobberRigid.velocity = Vector2.zero;

        _IsUsed = false;
    }
    public override bool IsUsing(ItemInterface itemInterface)
    {
        return itemInterface == ItemInterface.Equip;
    }
}
