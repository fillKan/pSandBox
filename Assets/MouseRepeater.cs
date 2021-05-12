using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMouseAction
{
    #region 설명 :
    /// <summary>
    /// 마우스 조작으로 취하게 될 동작을 담는 함수.
    /// </summary>
    #endregion
    void OperateAction(byte input);

    #region 설명 :
    /// <summary>
    /// 마우스로 특정 동작을 취할 오브젝트임을 등록하는 함수.
    /// 고정된 형식 사용 : MouseRepeater.Instance.AddActionObj(gameObject.GetInstanceID(),this);
    /// </summary>
    #endregion
    void RegisterAction();

    #region 설명 :
    /// <summary>
    /// 자신이 동작을 취할 수 있게되었을 때, 자신의 게임 오브젝트를 반환하는 함수.
    /// 고정된 형식 사용 : return gameObject;
    /// </summary>
    #endregion
    GameObject ActionObject();
}

public class MouseRepeater : Singleton<MouseRepeater>
{
    private Dictionary<int, IMouseAction> _actionObj = new Dictionary<int, IMouseAction>();
    public Dictionary<int,IMouseAction> ActionObj
    {
        get { return _actionObj; }
    }

    public void AddActionObj(int instanceID, IMouseAction action)
    {
        if(!_actionObj.ContainsKey(instanceID))
        {
            _actionObj.Add(instanceID, action);
        }
    }
}
