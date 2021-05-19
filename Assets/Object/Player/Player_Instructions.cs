using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 열거체 설명 : 
/// <summary>
/// 플레이어가 수행하는 지시들을 열거합니다.
/// <para>
/// 구성되는 각 열거자들의 설명은 FollowInstr함수를 사용할 때에 필요한 설명입니다.
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
    /// 특정 오브젝트를 추적하라는 지시. 필요한 인자 : gameObject
    /// </summary>
    #endregion
    GOTO_OBJECT,
    #region 설명 : 
    /// <summary>
    /// 상호작용 대상을 향해 이동하라는 지시. 필요한 인자 : GetInstanceID()
    /// </summary>
    #endregion
    GOTO_INSTR,
    #region 설명 : 
    /// <summary>
    /// 특정 오브젝트와 상호작용 하라는 지시. 필요한 인자 : GetInstanceID()
    /// </summary>
    #endregion
    DO_INTERACT
}

#region 열거체 설명 : 
/// <summary>
/// 지시를 수행하게되는 트리거들을 열거합니다.
/// <para>
/// 구성되는 각 열거자들의 설명은 ScheduleRunInstr함수를 사용할 때에 필요한 설명입니다.
/// </para>
/// </summary>
#endregion
public enum InstrTrigger
{
    #region 열거자 설명 :
    /// <summary>
    /// 해당 열거체의 첫 부분을 가리킵니다.
    /// <para>
    /// 실제로 하는 기능은 없습니다 ㄹㅇ
    /// </para>
    /// </summary>
    #endregion
    BEGIN,
    #region 열거자 설명 :
    /// <summary>
    /// 다음에 수행될 지시가 중단되지 않고서 종료되었을 때
    /// </summary>
    #endregion
    NEXT_INSTR_UNINTERRUPTED_DONE,

    #region 열거자 설명 :
    /// <summary>
    /// 해당 열거체의 끝 부분을 가리킵니다.
    /// <para>
    /// 실제로 하는 기능은 없습니다 진짜
    /// </para>
    /// </summary>
    #endregion
    END
}

#region 구조체 설명 :
/// <summary>
/// 플레이어가 수행중인 지시와, 지시의 코루틴을 포함하는 구조입니다.
/// </summary>
#endregion
public struct InstructionManager
{
    #region 설명 : 
    /// <summary>
    /// 현재 수행하고 있는 지시를 담는 변수.
    /// <para>만약 값이 NONE이라면, 수행중인 지시가 없다는 의미이다.</para>
    /// </summary>
    #endregion
    public Instructions instructions;
    #region 설명 : 
    /// <summary>
    /// 현재 플레이어가 수행중인 지시의 코루틴을 담는 변수.
    /// </summary>
    #endregion
    public IEnumerator progress;
}

public class Player_Instructions : Singleton<Player_Instructions>
{
    private InstructionManager progressInstr;
    private InstructionManager scheduleInstr;

    private Player player;

    private Boolean isCompletionInstr  = false;
    private Boolean isDisContinueInstr = false;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player);
    }

    #region 함수 설명 : 
    /// <summary>
    /// 플레이어가 특정한 지시를 수행하도록 합니다.
    /// </summary>
    /// <typeparam name="T">
    /// '수행할 지시에 필요한 값'의 자료형을 지정합니다
    /// </typeparam>
    /// <param name="directions">
    /// 플레이어가 수행할 지시를 지정합니다
    /// </param>
    /// <param name="xValue">
    /// 수행할 지시에 필요한 값 입니다
    /// </param>
    #endregion 
    public void FollowInstr<T>(Instructions instructions, T xValue)
    {
        isCompletionInstr  = false;
        isDisContinueInstr = false;

        Type type = InstrToType(instructions);

        //switch (instructions)
        //{
        //    case Instructions.GOTO_POINT:
        //        if (typeof(T).Equals(type))
        //        {
        //            DiscontinueInstr();
        //            progressInstr.instructions = Instructions.GOTO_POINT;

        //            Vector2 value;
        //                    value = (Vector2)Convert.ChangeType(xValue, typeof(Vector2));

        //                           progressInstr.progress = player.CR_moveMovementPoint(value);
        //            StartCoroutine(progressInstr.progress);
        //        }
        //        break;

        //    case Instructions.DO_INTERACT:
        //        if (typeof(T).Equals(type))
        //        {
        //            DiscontinueInstr();
        //            progressInstr.instructions = Instructions.DO_INTERACT;

        //            int value;
        //                value = (int)Convert.ChangeType(xValue, typeof(int));

        //                           progressInstr.progress = player.CR_Interaction(value);
        //            StartCoroutine(progressInstr.progress);
        //        }
        //        break;

        //    case Instructions.GOTO_OBJECT:
        //        if (typeof(T).Equals(type))
        //        {
        //            DiscontinueInstr();
        //            progressInstr.instructions = Instructions.GOTO_OBJECT;

        //            GameObject value;
        //                       value = (GameObject)Convert.ChangeType(xValue, typeof(GameObject));

        //                           progressInstr.progress = player.CR_moveMovementPoint(value);
        //            StartCoroutine(progressInstr.progress);
        //        }
        //        break;

        //    case Instructions.GOTO_INSTR:
        //        if (typeof(T).Equals(type))
        //        {
        //            DiscontinueInstr();
        //            progressInstr.instructions = Instructions.GOTO_INSTR;

        //            int value;
        //                value = (int)Convert.ChangeType(xValue, typeof(int));

        //                           progressInstr.progress = player.CR_moveMovementPoint(value);
        //            StartCoroutine(progressInstr.progress);
        //        }
        //        break;

        //    default:
        //        Debug.LogWarning("잘못된 지시입니다");
        //        break;
        //}
    }

    #region 함수 설명 : 
    /// <summary>
    /// 플레이어가 현재 수행하고있는 지시를 중단하도록 합니다.
    /// </summary>
    #endregion
    public void DiscontinueInstr()
    {
        isDisContinueInstr = true;

        progressInstr.instructions = Instructions.NONE;

        if (progressInstr.progress != null)
        {
            StopCoroutine(progressInstr.progress);

            progressInstr.progress = null;
        }
    }

    #region 함수 설명 : 
    /// <summary>
    /// 플레이어가 수행한 지시가 중단(DiscontinueInstr의 호출)없이 종료되었다면, 해당 함수를 호출합니다.
    /// <para>
    /// Player_Instructions에게 지시가 중단없이 종료되었다고 전달하는 기능입니다.
    /// </para>
    /// </summary>
    #endregion
    public void CompletionInstr()
    {
        isCompletionInstr = true;
    }

    #region 함수 설명 :
    /// <summary>
    /// 플레이어가 특정 조건에서 특정 지시를 수행하도록 설정합니다.
    /// </summary>
    /// <param name="trigger">
    /// 지시가 수행되는 특정 조건을 지정합니다
    /// </param>
    /// <param name="instructions">
    /// 수행할 지시를 지정합니다
    /// </param>
    /// <param name="xValue">
    /// 수행할 지시에 필요한 값입니다
    /// </param>
    #endregion
    public void ScheduleInstr<T>(InstrTrigger trigger, Instructions instructions, T xValue)
    {
        if(trigger > InstrTrigger.BEGIN && trigger < InstrTrigger.END)
        {
            if (!scheduleInstr.instructions.Equals(Instructions.NONE))
            {
                StopCoroutine(scheduleInstr.progress);
            }
            scheduleInstr.instructions = instructions;
            scheduleInstr.progress = ScheduleRunInstr(trigger, instructions, xValue);

            StartCoroutine(scheduleInstr.progress);
        }       
    }

    #region 함수 설명 :
    /// <summary>
    /// 인자값의 지시에 필요한 자료형의 Type을 반환합니다.
    /// </summary>
    /// <param name="instructions">
    /// 지시를 지정합니다.
    /// <para>
    /// 해당 값에따라 반환되는 값이 달라집니다.
    /// </para>
    /// </param>
    /// <returns></returns>
    #endregion
    public Type InstrToType(Instructions instructions)
    {
        switch (instructions)
        {
            case Instructions.NONE:
                return null;

            case Instructions.GOTO_POINT:
                return typeof(Vector2);

            case Instructions.GOTO_OBJECT:
                return typeof(GameObject);

            case Instructions.GOTO_INSTR:
            case Instructions.DO_INTERACT:
                return typeof(int);
        }

        return null;
    }

    #region 코루틴 설명 :
    /// <summary>
    /// 특정 조건이 이루어 지게되면, 설정된 지시를 수행하도록 합니다.
    /// </summary>
    /// <param name="trigger">
    /// 지시가 수행되는 특정 조건을 지정합니다
    /// </param>
    /// <param name="instructions">
    /// 수행할 지시를 지정합니다
    /// </param>
    /// <param name="xValue">
    /// 수행할 지시에 필요한 값입니다
    /// </param>
    #endregion
    private IEnumerator ScheduleRunInstr<T>(InstrTrigger trigger, Instructions instructions, T xValue)
    {
        switch (trigger)
        {
            case InstrTrigger.NEXT_INSTR_UNINTERRUPTED_DONE:
                Instructions nowProgressInstr = progressInstr.instructions;

                while (!isCompletionInstr)
                {
                    if (!progressInstr.instructions.Equals(nowProgressInstr) && isDisContinueInstr)
                    {
                        scheduleInstr.instructions = Instructions.NONE;
                        scheduleInstr.progress = null;

                        yield break;
                    }
                    yield return null;
                }
                FollowInstr(instructions, xValue);
                break;

            default:
                break;
        }
        yield break;
    }
}
