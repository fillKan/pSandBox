using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 클래스 설명 : 
/// <summary>
/// "땅을 벗어났는가?"의 여부를 판단하는 값을 알려주는 장치.
/// <para>
/// ※ BoxCollider2D -> isTrigger와 같이 사용하세요 ※
/// </para>
/// </summary>
#endregion
public class Brake : MonoBehaviour
{
    private bool  _exit = false;
    #region 변수 설명 : 
    /// <summary>
    /// "땅을 벗어났는가?" 의 여부를 알려주는 변수.
    /// </summary>
    #endregion
    public  bool  Exit
    {
        get { return _exit; }
    }

    private void OnTriggerExit2D (Collider2D other)
    {
        if(other.CompareTag("Ground"))
        {
            _exit = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            _exit = false;
        }
    }
}
