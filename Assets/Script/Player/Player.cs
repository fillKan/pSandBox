using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static readonly Vector3 FlipXScale = new Vector3(-1, 1, 1);
    private static readonly Vector3 DefaultScale = Vector3.one;

    public bool FlipX
    { get; private set; }
    private float MoveSpeed => PlayerStat.Instance.MoveSpeed;

    [SerializeField] private Inventory _Inventory;
    [SerializeField] private ItemFinder _Finder;
    [SerializeField] private float _InteractionRange = 1;

    private IEnumerator _CurrentOrderRoutine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            var closeItem = _Finder.GetCloseItem();
            if (closeItem != null)
            {
                InteractionOrder(closeItem);
            }
        }
        #region Moving
        {
            float axisX = Input.GetAxis("Horizontal");

            if (axisX != 0)
            {
                SetFlipX(axisX < 0);
                transform.Translate(Time.deltaTime * MoveSpeed * axisX, 0, 0);
            }
        }
        #endregion
    }
    private void InteractAction(InteractableObject target)
    {
        target.Interaction();

        ItemName item = _Inventory.EquipItemSlot.ContainItem;
        if (item != default)
        {
            var equipItem = ItemMaster.Instance.GetItemObject(item);
            if (equipItem.IsUsing(ItemInterface.Use))
            {
                equipItem.GetComponent<IUseItem>().UseItem(target);
            }
        }
    }
    public void SetFlipX(bool flipX)
    {
        FlipX = flipX;

        if (flipX) transform.localScale = FlipXScale;
        else transform.localScale = DefaultScale;
    }
    public void OrderCancel()
    {
        if (_CurrentOrderRoutine != null) {
            StopCoroutine(_CurrentOrderRoutine);
        }
        _CurrentOrderRoutine = null;
    }
    public void InteractionOrder(InteractableObject target)
    {
        OrderCancel();

        var distance = Mathf.Abs(target.transform.position.x - transform.position.x);
        if (distance > _InteractionRange)
        {
            StartCoroutine(_CurrentOrderRoutine = TraceRoutine(target.transform, 
                () => { return Mathf.Abs(target.transform.position.x - transform.position.x) > _InteractionRange; }, 
                () => { InteractAction(target); }));
        }
        else
        {
            InteractAction(target);
        }
    }
    public void MoveToPointOrder(Vector2 point)
    {
        OrderCancel();

        StartCoroutine(_CurrentOrderRoutine = MoveRoutine(point));
    }
    private IEnumerator TraceRoutine(Transform target, Func<bool> canTracing, Action tracingDone = null)
    {
        while (canTracing.Invoke())
        {
            float direction = (target.position.x > transform.position.x ? 1f : -1f);
            transform.position += Vector3.right * direction * Time.deltaTime * MoveSpeed;

            SetFlipX(direction < 0);

            yield return null;
        }
        _CurrentOrderRoutine = null;
        tracingDone?.Invoke();
    }
    private IEnumerator MoveRoutine(Vector2 point)
    {
        float direction = (point.x > transform.position.x ? 1f : -1f);
        SetFlipX(direction < 0);

        while (gameObject.activeInHierarchy)
        {
            transform.position += Vector3.right * direction * Time.deltaTime * MoveSpeed;

            if ((direction < 0f && transform.position.x < point.x) ||
                (direction > 0f && transform.position.x > point.x))
            {
                transform.position = new Vector3
                    (point.x, transform.position.y, transform.position.z);

                break;
            }
            yield return null;
        }
        _CurrentOrderRoutine = null;
    }
}
