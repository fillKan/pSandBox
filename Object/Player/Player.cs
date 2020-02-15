using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 인터페이스 설명 :
/// <summary>
/// 플레이어와의 상호작용 인터페이스 
/// </summary>
#endregion
public interface Interaction
{
    #region 설명 :
    /// <summary>
    /// 플레이어와의 상호작용을 하는 함수
    /// </summary>
    #endregion
    void OperateAction();

    #region 설명 :
    /// <summary>
    /// 플레이어와 상호작용할 오브젝트임을 플레이어에게 알려주는 함수.
    /// 고정된 형식 사용 : PlayerGetter.Instance.AddInteractObj(gameObject.GetInstanceID(),this);
    /// </summary>
    #endregion
    void RegisterInteraction();

    #region 설명 :
    /// <summary>
    /// 상호작용의 대상으로 지정된 오브젝트를 반환하는 함수
    /// 고정된 형식 사용 : return gameObject;
    /// </summary>
    #endregion
    GameObject InteractObject();
}

#region 구조체 설명 :
/// <summary>
/// 플레이어가 수행중인 지시와, 지시의 코루틴을 포함하는 구조체. 
/// </summary>
#endregion
public struct ProgressInstr
{
    #region 설명 : 
    /// <summary>
    /// 현재 수행하고 있는 지시를 담는 변수.
    /// <para>만약 값이 NONE이라면, 수행중인 지시가 없다는 의미이다.</para>
    /// </summary>
    #endregion
    public Player.Instructions instructions;
    #region 설명 : 
    /// <summary>
    /// 현재 플레이어가 수행중인 지시의 코루틴을 담는 변수.
    /// </summary>
    #endregion
    public IEnumerator progress;
}

public class Player : MonoBehaviour
{
    #region 열거체 설명 : 
    /// <summary>
    /// 플레이어에게 지시하는 행동들의 종류를 담는다.
    /// <para>
    /// 구성되는 열거자들의 설명은 FollowInstr함수를 사용할 때에 필요한 설명이다.
    /// </para>
    /// </summary>
    #endregion
    public enum Instructions
    {
        #region 설명 : 
        /// <summary>
        /// 이행중인 지시를 중단하라는 지시. 또는 그런 상태.
        /// </summary>
        #endregion
        NONE,
        #region 설명 : 
        /// <summary>
        /// 특정 지점으로 이동하라는 지시. 필요한 인자 : Vector2
        /// </summary>
        #endregion
        GOTO_POINT,
        #region 설명 : 
        /// <summary>
        /// 특정 오브젝트와 상호작용 하라는 지시. 필요한 인자 : GetInstanceID()
        /// </summary>
        #endregion
        DO_INTERACT
    }
    public Inventory Inventory;

    private Vector2 vDir;
    private SpriteRenderer sprite;

    private ProgressInstr progressInstr;

    #region 설명 :
    /// <summary>
    /// 플레이어가 상호작용 대상으로 이동하는 코루틴을 담는다.
    /// </summary>
    #endregion
    private IEnumerator moveInteractionPoint;

    #region 설명 :
    /// <summary>
    /// 플레이어가 특정 지점으로 이동하게끔 하는 코루틴을 담는 열거체. 
    /// </summary>
    #endregion
    private IEnumerator moveMovementPoint;

    private Dictionary<int, Interaction> _interactObj = new Dictionary<int, Interaction>();
    public Dictionary<int, Interaction> InteractObj
    {
        get { return _interactObj; }
    }

    public void AddInteractObj(int instanceID, Interaction interaction)
    {
        if(!_interactObj.ContainsKey(instanceID))
        {
            _interactObj.Add(instanceID, interaction);
        }
    }

    #region 함수 설명 :
    /// <summary>
    /// 플레이어에게 상호작용을 지시하는 함수.
    /// </summary>
    /// <param name="instanceID">
    /// 상호작용할 오브젝트의 GetInstanceID()를 담느다.
    /// </param>
    #endregion
    private void InteractCommend(int instanceID)
    {
        if(_interactObj.ContainsKey(instanceID))
        {
            progressInstr.progress = CR_moveInteractionPoint(instanceID);

            StartCoroutine(progressInstr.progress);
        }
    }

    #region 함수 설명 :
    /// <summary>
    /// 플레이어에게 특정 지점으로 이동할 것을 지시하는 함수.
    /// </summary>
    /// <param name="targetPoint">
    /// 플레이어가 이동할 특정 지점.
    /// </param>
    #endregion
    private void MovementCommend(Vector2 targetPoint)
    {
        progressInstr.progress = CR_moveMovementPoint(targetPoint);

        StartCoroutine(progressInstr.progress);
    }

    #region 함수 설명 : 
    /// <summary>
    /// 외부에서 플레이어에게 특정 행동을 지시를 하는 허브
    /// </summary>
    /// <typeparam name="T">
    /// 지시하는 행동에 필요한 인자의 자료형.
    /// </typeparam>
    /// <param name="directions">
    /// 플레이어에게 지시할 특정한 행동.
    /// </param>
    /// <param name="xValue">
    /// 지시하는 행동에 필요한 인자 값.
    /// </param>
    #endregion 
    public void FollowInstr<T>(Instructions instructions, T xValue) 
    {
        switch (instructions)
        {
            case Instructions.GOTO_POINT:
                
                if(typeof(T).Equals(typeof(Vector2)))
                {
                    DiscontinueInstr();
                    progressInstr.instructions = Instructions.GOTO_POINT;

                    Vector2 value;
                            value = (Vector2)Convert.ChangeType(xValue, typeof(Vector2));

                    MovementCommend(value);
                }
                break;

            case Instructions.DO_INTERACT:

                if (typeof(T).Equals(typeof(int)))
                {
                    DiscontinueInstr();
                    progressInstr.instructions = Instructions.DO_INTERACT;

                    int value;
                        value = (int)Convert.ChangeType(xValue, typeof(int));

                    InteractCommend(value);
                }
                break;

            default:
                Debug.LogWarning("잘못된 지시입니다");
                break;
        }
    }
    #region 함수 설명 : 
    /// <summary>
    /// 외부에서 플레이어에게 수행중인 지시를 중단시키는 함수.
    /// </summary>
    /// <param name="directions">
    /// 인자로 NONE를 사용한다.
    /// </param>
    #endregion 
    public void FollowInstr(Instructions instructions)
    {
        if(instructions.Equals(Instructions.NONE))
        {
            DiscontinueInstr();
        }
    }

    private void DiscontinueInstr()
    {
        progressInstr.instructions = Instructions.NONE;

        if(progressInstr.progress != null)
        {
            StopCoroutine(progressInstr.progress);
        }
        progressInstr.progress = null;
    }

    private void OnEnable()
    {
        vDir = transform.position;

        sprite = gameObject.GetComponent<SpriteRenderer>();

        StartCoroutine(CR_update());
    }

    private IEnumerator CR_update()
    {
        while (gameObject.activeSelf)
        {
            // 이동 방향은 현재 플레이어의 위치로 계속해서 초기화한다.
            vDir = transform.position;

            if (Input.GetAxis("Horizontal") != 0)
            {
                // 수행중인 지시를 중단한다.
                DiscontinueInstr();

                vDir.x += Input.GetAxis("Horizontal") * Time.deltaTime * 3.5f;

                if (Input.GetAxis("Horizontal") > 0) sprite.flipX = false;
                if (Input.GetAxis("Horizontal") < 0) sprite.flipX = true;

                transform.position = vDir;
            }

            yield return null;
        }

        yield break;
    }

    #region 코루틴 설명
    /// <summary>
    /// 플레이어가 상호작용 대상으로 이동하는 코루틴.
    /// </summary>
    /// <param name="interactObj">
    /// 상호작용할 오브젝트의 GetInstanceID()를 담느다.
    /// </param>
    #endregion
    private IEnumerator CR_moveInteractionPoint(int interactObj)
    {
        float moveAmount = 0;
        #region 변수 설명 :
        /*
         *  posX       : 상호작용 대상의 x좌표
         *  moveAmount : 점차 증가할 이동 속도를 담는 변수
         */
        #endregion

        // 만약 이동 대상의 x좌표가 더 크다면?
        if (_interactObj[interactObj].InteractObject().transform.position.x > transform.position.x + 0.75f)
        {
            // 스프라이트 전환
            sprite.flipX = false;

            // 서로의 x축이 0.75정도 차이날 때 까지 이동한다.
            while (_interactObj[interactObj].InteractObject().transform.position.x > transform.position.x + 0.75f)
            {
                // moveAmount는 서서히 증가한다.
                if (moveAmount < 1) moveAmount += 0.06f;

                // 이동!
                vDir.x += moveAmount * Time.deltaTime * 3.5f;
                transform.position = vDir;

                yield return null;
            }

            // 뚝 끊기는 움직임을 방지하기 위해, 목표지점에 도달하고 나서도 이동한다. 
            while(moveAmount > 0)
            {
                // 서서히 감속
                moveAmount -= 0.16f;

                // 이동!
                vDir.x += moveAmount * Time.deltaTime * 3.5f;
                transform.position = vDir;

                yield return null;
            }
        }

        // 만약 이동 대상의 x좌표가 더 작다면?
        else if (_interactObj[interactObj].InteractObject().transform.position.x < transform.position.x - 0.75f)
        {
            // 스프라이트 전환
            sprite.flipX = true;

            // 상호작용 대상과의 x좌표가 0.75차이날 때 까지 이동한다.
            while (_interactObj[interactObj].InteractObject().transform.position.x < transform.position.x - 0.75f)
            {
                // 서서히 가속...
                if (moveAmount > -1) moveAmount -= 0.06f;

                // 이동!
                vDir.x += moveAmount * Time.deltaTime * 3.5f;
                transform.position = vDir;

                yield return null;
            }

            // 뚝 끊기는 움직임을 방지하기 위해, 목표지점에 도달하고 나서도 이동한다.
            while (moveAmount < 0)
            {
                // 서서히 감속
                moveAmount += 0.16f;

                // 이동!
                vDir.x += moveAmount * Time.deltaTime * 3.5f;
                transform.position = vDir;

                yield return null;
            }
        }

        moveInteractionPoint = null;

        StartCoroutine(CR_Vibration(0.06f, 0.25f));
        _interactObj[interactObj].OperateAction();

        yield break;
    }

    private IEnumerator CR_moveMovementPoint(Vector2 targetPoint)
    {
        float fMoveAmount = 0;

        if(targetPoint.x > transform.position.x)
        {
            sprite.flipX = false;

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
            sprite.flipX = true;

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

        moveMovementPoint = null;
        
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
}
