using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static readonly Vector3 FlipXScale = new Vector3(-1, 1, 1);
    private static readonly Vector3 DefaultScale = Vector3.one;

    public const float MoveSpeed = 3.5f;

    public Inventory Inventory;

    [Tooltip("플레이어가 근처 아이템을 감지할 때 사용되는 레이더입니다.")]
    public ItemFinder Finder;
    public bool FlipX
    { get; private set; }

    #region 변수 설명 : 
    /// <summary>
    /// 플레이어의 상호작용 범위를 지정합니다.
    /// </summary>
    #endregion
    [Tooltip("플레이어의 상호작용 범위를 지정합니다.")]
    public float InteractionRange = 1;

    [Tooltip("플레이어가 현재 프레임에 장비한 아이템 슬롯들을 담는 배열")]
    public  List<ItemSlot> EquipItemSlots    = new List<ItemSlot>();
    [Tooltip("플레이어가 이전 프레임에 장비한 아이템 슬롯들을 담는 배열")]
    private List<ItemName> EquippedItemSlots = new List<ItemName>();


    private IEnumerator _CurrentOrderRoutine;

    #region 함수 설명 : 
    /// <summary>
    /// 플레이어가 들고있는 아이템들을 모두 사용하는 함수입니다.
    /// </summary>
    /// <param name="interactionID">
    /// 플레이어가 상호작용한 대상의 GetInstanceID를 지정합니다   
    /// </param>
    #endregion
    private void UseItem(int interactionID)
    {
        var target = InteractionManager.Instance[interactionID];
        if (target == null) return;

        for (int i = 0; i < EquipItemSlots.Count; ++i)
        {
            if (EquipItemSlots[i])
            {
                if (EquipItemSlots[i].ContainItem != ItemName.NONE)
                {
                    var item = ItemMaster.Instance.GetItem(EquipItemSlots[i].ContainItem);
                    if (item.IsUsing(ItemInterface.Use))
                    {
                        item.GetComponent<IUseItem>()?.UseItem(target);
                    }
                }
            }
        }
       
    }

    private void OnEnable()
    {
        for (int i = 0; i < EquipItemSlots.Count; i++)
        {
            EquippedItemSlots.Add(ItemName.NONE);
        }
        StateStorage.Instance.IncreaseState(States.TREE_LOGGING, 1);
    }

    private void Update()
    {
        while (gameObject.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                var closeItem = Finder.GetCloseItem();
                if (closeItem != null)
                {
                    InteractionOrder(closeItem);
                }
            }
            #region Moving
            {
                float axisX = Input.GetAxis("Horizontal");
                if (axisX > 0)
                {                   
                    SetFlipX(false);
                }
                else if (axisX < 0)
                {
                    SetFlipX(true);
                }
                transform.Translate(Time.deltaTime * MoveSpeed * axisX, 0, 0);
            }
            #endregion
        }
    }
    private void InteractAction(InteractableObject target)
    {
        target.Interaction();

        for (int i = 0; i < EquipItemSlots.Count; i++)
        {
            ItemName item = EquipItemSlots[i].ContainItem;
            if (item != default)
            {
                var equipItem = ItemMaster.Instance.GetItemObject(item);
                if (equipItem.IsUsing(ItemInterface.Use))
                {
                    equipItem.GetComponent<IUseItem>().UseItem(target);
                }
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
        if (distance > InteractionRange)
        {
            StartCoroutine(_CurrentOrderRoutine = TraceRoutine(target.transform, 
                () => { return Mathf.Abs(target.transform.position.x - transform.position.x) > InteractionRange; }, 
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
