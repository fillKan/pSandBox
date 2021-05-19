﻿using System;
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

    private Vector3 vDir;

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
        vDir = transform.position;

        for(int i = 0; i < EquipItemSlots.Count; i++)
        {
            EquippedItemSlots.Add(ItemName.NONE);
        }
        StateStorage.Instance.IncreaseState(States.TREE_LOGGING, 1);

        StartCoroutine(UpdateRoutine());
    }

    private IEnumerator UpdateRoutine()
    {
        while (gameObject.activeInHierarchy)
        {
            vDir = transform.position;

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
                Player_Instructions.Instance.DiscontinueInstr();

                if (Input.GetAxis("Horizontal") > 0)
                {                   
                    SetFlipX(false);
                }
                else if (Input.GetAxis("Horizontal") < 0)
                {
                    SetFlipX(true);
                }
                vDir.x += Time.deltaTime * 3.5f * Input.GetAxis("Horizontal");

                transform.position = vDir;
            }
            #endregion
            yield return null;
        }
    }

    #region 코루틴 설명
    /// <summary>
    /// 플레이어가 상호작용 대상으로 이동하는 코루틴.
    /// </summary>
    /// <param name="interactObj">
    /// 상호작용할 오브젝트의 GetInstanceID()를 담느다.
    /// </param>
    #endregion
    public IEnumerator CR_Interaction(int interactObj)
    {
        // 플레이어와 상호작용 대상과의 거리가 InteractionRange보다 작다면, 상호작용 대상을 향해 이동한다.
        if (AbsDistance(Player_Interaction.Instance.InObjGetValue(interactObj).InteractObject().transform.position.x, transform.position.x) > InteractionRange)
        {
            Player_Instructions.Instance.FollowInstr(Instructions.GOTO_INSTR, interactObj);

            Player_Instructions.Instance.ScheduleInstr(InstrTrigger.NEXT_INSTR_UNINTERRUPTED_DONE, Instructions.DO_INTERACT, interactObj);

            yield break;
        }

        StartCoroutine(CR_Vibration(0.06f, 0.25f));

        bool isSlotEmpty = true;

        for (int i = 0; i < EquipItemSlots.Count; i++)
        {
            if (EquipItemSlots[i].ContainItem != ItemName.NONE)
            {
                UseItem(interactObj);

                isSlotEmpty = false;

                break;
            }
        }
        if(isSlotEmpty)
        {
            Player_Interaction.Instance.InObjGetValue(interactObj).OperateAction<IItemFunction>(null);
        }
        

        Player_Instructions.Instance.CompletionInstr();
        yield break;
    }

    #region 코루틴 설명
    /// <summary>
    /// 플레이어가 지정한 위치로 이동하는 코루틴입니다.
    /// </summary>
    /// <param name="targetPoint">
    /// 이동할 지점을 지정합니다
    /// </param>
    #endregion
    public IEnumerator CR_moveMovementPoint(Vector2 targetPoint)
    {
        float fMoveAmount = 0;

        if(targetPoint.x > transform.position.x)
        {
            SetFlipX(false);

            while (targetPoint.x > transform.position.x)
            {
                if (fMoveAmount < 1)
                {
                    fMoveAmount += 0.06f;
                }
                vDir.x += fMoveAmount * Time.deltaTime * 3.5f;

                transform.position = vDir;

                yield return null;
            }
            while (fMoveAmount > 0)
            {
                fMoveAmount -= 0.16f;

                vDir.x += fMoveAmount * Time.deltaTime * 3.5f;
                transform.position = vDir;

                yield return null;
            }
        }

        else if (targetPoint.x < transform.position.x)
        {
            SetFlipX(true);

            while (targetPoint.x < transform.position.x)
            {
                if (fMoveAmount < 1)
                {
                    fMoveAmount += 0.06f;
                }
                vDir.x -= fMoveAmount * Time.deltaTime * 3.5f;

                transform.position = vDir;

                yield return null;
            }
            while (fMoveAmount > 0)
            {
                fMoveAmount -= 0.16f;

                vDir.x -= fMoveAmount * Time.deltaTime * 3.5f;
                transform.position = vDir;

                yield return null;
            }
        }
        Player_Instructions.Instance.CompletionInstr();
        yield break;
    }

    #region 코루틴 설명
    /// <summary>
    /// 플레이어가 지정한 오브젝트를 추적하는 코루틴입니다.
    /// </summary>
    /// <param name="target">
    /// 추적할 오브젝트를 지정합니다.
    /// </param>
    #endregion
    public IEnumerator CR_moveMovementPoint(GameObject target)
    {
        float fMoveAmount = 0;
        Transform Target  = target.transform;

        if (Target.position.x > transform.position.x)
        {
            SetFlipX(false);

            while (Target.position.x > transform.position.x)
            {
                if (fMoveAmount < 1)
                {
                    fMoveAmount += 0.06f;
                }
                vDir.x += fMoveAmount * Time.deltaTime * 3.5f;

                transform.position = vDir;

                yield return null;
            }
            while (fMoveAmount > 0)
            {
                fMoveAmount -= 0.16f;

                vDir.x += fMoveAmount * Time.deltaTime * 3.5f;
                transform.position = vDir;

                yield return null;
            }
        }

        else if (Target.position.x < transform.position.x)
        {
            SetFlipX(true);

            while (Target.position.x < transform.position.x)
            {
                if (fMoveAmount < 1)
                {
                    fMoveAmount += 0.06f;
                }
                vDir.x -= fMoveAmount * Time.deltaTime * 3.5f;

                transform.position = vDir;

                yield return null;
            }
            while (fMoveAmount > 0)
            {
                fMoveAmount -= 0.16f;

                vDir.x -= fMoveAmount * Time.deltaTime * 3.5f;
                transform.position = vDir;

                yield return null;
            }
        }

        Player_Instructions.Instance.CompletionInstr();
        yield break;
    }

    #region 코루틴 설명
    /// <summary>
    /// 플레이어가 지정한 상호작용 대상을 향해 이동하는 코루틴입니다.
    /// </summary>
    /// <param name="interactionID">
    /// 상호작용할 대상의 GetInstanceID()를 지정합니다
    /// </param>
    #endregion
    public IEnumerator CR_moveMovementPoint(int interactionID)
    {
        float fMoveAmount = 0;
        Transform IntractObj = Player_Interaction.Instance.InObjGetValue(interactionID).InteractObject().transform;

        if (IntractObj.position.x > transform.position.x + InteractionRange)
        {
            SetFlipX(false);

            while (IntractObj.position.x > transform.position.x + InteractionRange)
            {
                if (fMoveAmount < 1)
                {
                    fMoveAmount += 0.06f;
                }
                vDir.x += fMoveAmount * Time.deltaTime * 3.5f;

                transform.position = vDir;

                yield return null;
            }
            while (fMoveAmount > 0)
            {
                fMoveAmount -= 0.16f;

                vDir.x += fMoveAmount * Time.deltaTime * 3.5f;
                transform.position = vDir;

                yield return null;
            }
        }

        else if (IntractObj.position.x < transform.position.x - InteractionRange)
        {
            SetFlipX(true);

            while (IntractObj.position.x < transform.position.x - InteractionRange)
            {
                if (fMoveAmount < 1)
                {
                    fMoveAmount += 0.06f;
                }
                vDir.x -= fMoveAmount * Time.deltaTime * 3.5f;

                transform.position = vDir;

                yield return null;
            }
            while (fMoveAmount > 0)
            {
                fMoveAmount -= 0.16f;

                vDir.x -= fMoveAmount * Time.deltaTime * 3.5f;
                transform.position = vDir;

                yield return null;
            }
        }
        Player_Instructions.Instance.CompletionInstr();
        yield break;
    }

    private IEnumerator CR_Vibration(float amount, float time)
    {
        Vector2 vInitPos = transform.position;

        while(time > 0)
        {
            time -= Time.deltaTime;

            transform.position = ((Vector2)UnityEngine.Random.insideUnitSphere * amount) + vInitPos;

            yield return null;
        }
        transform.position = vInitPos;

        yield break;
    }

    #region 함수 설명 :
    /// <summary>
    /// 수직선상 두 점 사이 거리의 절댓값을 반환합니다.
    /// </summary>
    /// <param name="pointA">
    /// 수직선상에 위치한 한 점입니다.
    /// </param>
    /// <param name="pointB">
    /// 수직선상에 위치한 한 점입니다.
    /// </param>
    /// <returns></returns>
    #endregion
    private float AbsDistance(float pointA, float pointB)
    {
        if(pointA > pointB)
        {
            return pointA - pointB;
        }
        else if (pointA < pointB)
        {
            return pointB - pointA;
        }

        return 0;
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
