using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tree : MonoBehaviour
{
    #region 변수 설명 : 
    /// <summary>
    /// 해당 나무의 SpriteRenderer를 불러옵니다.
    /// </summary>
    #endregion
    public    SpriteRenderer  SprtRenderer
    {
        get { return _sprtRenderer; }
    }
    protected SpriteRenderer _sprtRenderer;

    #region 변수 설명 : 
    /// <summary>
    /// 해당 나무를 베고 있는지의 여부를 불러옵니다.
    /// </summary>
    #endregion
    public    bool  DoingChopTree
    {
        get { return _doingChopTree; }
    }
    protected bool _doingChopTree = false;

    #region 변수 설명 : 
    /// <summary>
    /// 해당 나무가 쓰러지고 있는지의 여부를 불러옵니다.
    /// </summary>
    #endregion
    public    bool  IsCutDown
    {
        get { return _isCutDown; }
    }
    protected bool _isCutDown = false;

    #region 변수 설명 : 
    /// <summary>
    /// 해당 나무의 내구도를 불러옵니다.
    /// </summary>
    #endregion
    public    float  fDurability
    {
        get { return _fDurability; }
    }
    protected float _fDurability;

    private void Start()
    {
        InitTree();
    }

    #region 코루틴 설명 : 
    /// <summary>
    /// 나무를 뱁니다.
    /// </summary>
    /// <param name="cutAmount">
    /// 해당 값만큼 나무의 내구도가 줄어듦니다.
    /// </param>
    /// <param name="vibTime">
    /// 해당 값만큼 나무를 베는데에 딜레이가 걸리며, 나무가 흔들립니다.
    /// </param>
    /// <param name="vibAmount">
    /// 나무가 흔들리는 정도를 나타냅니다.
    /// </param>
    /// <returns></returns>
    #endregion
    public IEnumerator CR_chopTree(float cutAmount, float vibTime, float vibAmount)
    {
        _doingChopTree = true;
        _fDurability  -= cutAmount;

        Vector2 vInitPos = transform.position;
        
        while (vibTime > 0)
        {
            vibTime -= Time.deltaTime;
            
            transform.position = ((Vector2)Random.insideUnitSphere * vibAmount) + vInitPos;
            yield return null;
        }
        transform.position = vInitPos;
        _doingChopTree     = false;
        yield break;
    }

    #region 코루틴 설명 : 
    /// <summary>
    /// 나무가 서서히 사라지며, 끝내 비활성화 됩니다.
    /// </summary>
    #endregion
    public IEnumerator CR_cutDown()
    {
        _isCutDown  = true;
        float alpha = 1;

        while (SprtRenderer.color.a > 0)
        {
            alpha -= 0.02f;
            SprtRenderer.color = new Color(1, 1, 1, alpha);

            yield return null;
        }
        gameObject.SetActive(false);
        _isCutDown = false;

        yield break;
    }

    #region 설명 :
    /// <summary>
    /// 나무를 베고나서 드롭되는 아이템을 담는다.
    /// </summary>
    #endregion
    public abstract void DropItem();

    #region 설명 :
    /// <summary>
    /// 오브젝트가 활성화 될 때 실행된다.(초기화)
    /// </summary>
    #endregion
    protected abstract void InitTree();
}
