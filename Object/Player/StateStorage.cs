using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 열거체 설명 :
/// <summary>
/// 플레이어의 각종 능력치들을 열거합니다.
/// </summary>
#endregion
public enum States
{
    TREE_LOGGING
}

#region 클래스 설명 :
/// <summary>
/// 플레이어의 각종 능력치들의 수치를 저장하는 객체입니다.
/// </summary>
#endregion
public class StateStorage : Singleton<StateStorage>
{
    #region 변수 설명 :
    /// <summary>
    /// 플레이어가 벌목으로 나무의 내구도를 감소시키는 수치입니다.
    /// </summary>
    #endregion
    public  float  TreeLogging
    {
        get
        {
            return storage[States.TREE_LOGGING];
        }
    }

    private Dictionary<States, float> storage = new Dictionary<States, float>();

    private void Awake()
    {
        storage.Add(States.TREE_LOGGING, 0);
    }

    #region 함수 설명 :
    /// <summary>
    /// 지정한 능력치를 인자값만큼 증가시킵니다.
    /// </summary>
    /// <param name="state">
    /// 증가시킬 능력치를 지정합니다
    /// </param>
    /// <param name="figure">
    /// 증가시킬 수치입니다
    /// </param>
    #endregion
    public void IncreaseState(States state, float figure)
    {
        if(storage.ContainsKey(state))
        {
            storage[state] += figure;
        }
    }

    #region 함수 설명 :
    /// <summary>
    /// 지정한 능력치를 인자값만큼 감소시킵니다.
    /// </summary>
    /// <param name="state">
    /// 감소시킬 능력치를 지정합니다
    /// </param>
    /// <param name="figure">
    /// 감소시킬 수치입니다
    /// </param>
    #endregion
    public void DecreaseState(States state, float figure)
    {
        if (storage.ContainsKey(state))
        {
            storage[state] -= figure;
        }
    }
}
