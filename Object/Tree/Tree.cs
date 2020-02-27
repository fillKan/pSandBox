using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tree : MonoBehaviour
{
    public SpriteRenderer SprtRenderer;
    //protected SpriteRenderer _sprtRenderer;

    public bool DoingChopTree = false;
    //protected bool _doingChopTree = false;

    public float fDurability = 0;
    //protected float _fDurability;

    private void Start()
    {
        InitTree();
    }

    // 0.4 0.1
    public IEnumerator CR_vibration(float time, float amount)
    {
        Vector2 vInitPos = transform.position;
        
        while (time > 0)
        {
            time -= Time.deltaTime;
            
            transform.position = ((Vector2)Random.insideUnitSphere * amount) + vInitPos;
            yield return null;
        }
        transform.position = vInitPos;

        yield break;
    }

    public IEnumerator CR_fade()
    {
        float alpha = 1;

        while (SprtRenderer.color.a > 0)
        {
            alpha -= 0.02f;
            SprtRenderer.color = new Color(1, 1, 1, alpha);

            yield return null;
        }

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
