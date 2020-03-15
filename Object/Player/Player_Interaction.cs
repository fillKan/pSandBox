using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 인터페이스 설명 :
/// <summary>
/// 플레이어와 상호작용하는 인터페이스입니다. 
/// </summary>
#endregion
public interface Interaction
{
    #region 설명 :
    /// <summary>
    /// 플레이어와의 상호작용을 하는 함수
    /// </summary>
    #endregion
    void OperateAction<T>(T xValue) where T : ItemFunction;

    #region 설명 :
    /// <summary>
    /// 플레이어와 상호작용할 오브젝트임을 플레이어에게 알려주는 함수.
    /// 고정된 형식 사용 : Player_Interaction.Instance.InObjRegister(gameObject.GetInstanceID(), this);
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

public class Player_Interaction : Singleton<Player_Interaction>
{
    private Dictionary<int, Interaction> InObjDirectory = new Dictionary<int, Interaction>();

    #region 함수 설명 : 
    /// <summary>
    /// 플레이어와 상호작용하는 오브젝트를 등록합니다.
    /// </summary>
    /// <param name="key">
    /// 등록할 오브젝트의 GetInstanceID를 지정합니다   
    /// </param>
    /// <param name="interaction">
    /// 등록할 오브젝트의 Interaction인터페이스를 지정합니다
    /// </param>
    #endregion
    public void InObjRegister(int key, Interaction interaction)
    {
        if(!InObjDirectory.ContainsKey(key))
        {
            InObjDirectory.Add(key,interaction);
        }
    }

    #region 함수 설명 : 
    /// <summary>
    /// 상호작용하는 오브젝트인지의 여부를 반환합니다. 
    /// </summary>
    /// <param name="key">
    /// 여부를 확인할 오브젝트의 GetInstanceID를 지정합니다.   
    /// </param>
    #endregion
    public bool InObjCheck(int key)
    {
        return InObjDirectory.ContainsKey(key);
    }

    #region 함수 설명 : 
    /// <summary>
    /// 인자값과 대치되는 상호작용 오브젝트를 반환합니다. 
    /// </summary>
    /// <param name="key">
    /// 반환할 오브젝트의 GetInstanceID를 지정합니다.   
    /// </param>
    #endregion
    public Interaction InObjGetValue(int key)
    {
        return InObjDirectory[key];
    }
}
