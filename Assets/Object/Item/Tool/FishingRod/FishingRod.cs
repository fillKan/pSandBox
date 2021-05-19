using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : Item, IEquipItem
{
    private const int Default = 0;
    private const int ThrowBobber = 1;
    private const int CatchBobber = 2;

    private const float MaxLineLength = 15f;

    [Header("FishingRod Property")]
    [SerializeField] private Sprite _UsedSprite;
    [SerializeField] private Sprite _DefaultSprite;
    [SerializeField] private SpriteRenderer _Renderer;

    [SerializeField] private Animator _Animator;
    private int _AnimControlKey;
    [SerializeField] private LineRenderer _LineRenderer;
    [SerializeField] private Transform _RodTopPoint;

    [Header("Bobber Property")]
    [SerializeField] private Bobber _Bobber;
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
        _AnimControlKey = _Animator.GetParameter(0).nameHash;
    }
    public void DisEquipItem()
    {
        _IsEquiped = false;
        BobberDisable();
    }
    public override bool IsUsing(ItemInterface itemInterface)
    {
        return itemInterface == ItemInterface.Equip;
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
                        _Animator.SetInteger(_AnimControlKey, CatchBobber);
                    }
                    else
                    {
                        _Animator.SetInteger(_AnimControlKey, ThrowBobber);
                    }
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(_Bobber.gameObject) && !_IsUsed)
        {
            _Bobber.DropFish();
            BobberDisable();
        }
    }
    private void BobberDisable()
    {
        _Renderer.sprite = _DefaultSprite;

        _Bobber.transform.SetParent(transform);
        _Bobber.gameObject.SetActive(false);
        _LineRenderer.gameObject.SetActive(false);

        _Bobber.Rigidbody.velocity = Vector2.zero;

        _IsUsed = false;
    }
    private void AE_ThrowBobber()
    {
        _Renderer.sprite = _UsedSprite;

        _Bobber.transform.position = _RodTopPoint.position;
        _Bobber.transform.SetParent(null);
        _Bobber.gameObject.SetActive(true);

        Vector3 bobberPos = _Bobber.transform.position;
        Vector3 rodTopPos = _RodTopPoint.position;

        _LineRenderer.SetPosition(0, rodTopPos);
        _LineRenderer.SetPosition(1, bobberPos);
        _LineRenderer.gameObject.SetActive(true);

        Vector2 dir = (_Cursor.transform.position - rodTopPos);
        _Bobber.Rigidbody.AddForce(dir.normalized * _ThrowingForce * dir.magnitude);

        _IsUsed = !_IsUsed;
    }
    private void AE_CatchBobber()
    {
        _Bobber.CatchFish();

        Vector2 dir = (_RodTopPoint.position - _Bobber.transform.position);
        Vector2 force = dir.normalized * _ThrowingForce * dir.magnitude;

        _Bobber.Rigidbody.velocity = force;
        _Bobber.Rigidbody.AddForce(force);

        _IsUsed = !_IsUsed;
    }
    private void AE_AnimPlayOver()
    {
        _Animator.SetInteger(_AnimControlKey, Default);
    }
}
