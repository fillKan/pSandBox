using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tree : MonoBehaviour
{
    protected SpriteRenderer sprite;

    protected bool doingChopTree = false;
    protected float fDurability;

    #region 변수 설명 :
    /*
    *  sprite        : 나무의 SpriteRenderer컴포넌트를 담는 변수
    *  doingChopTree : 나무를 베고있는 중인지를 담는 변수
    *  fDurability   : 나무의 내구도를 담는 변수
    */
    #endregion


    private void OnEnable()
    {
        InitTree();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Space) && !doingChopTree)
            {
                StartCoroutine(CR_chopTree());
            }
        }
    }

    protected virtual IEnumerator CR_chopTree()
    {
        doingChopTree = true;
        fDurability  -= 3;

        #region 변수 설명

        float fTime = 0.4f;
        Vector2 vInitPos = transform.position;

        /*
         * 오브젝트 흔들기의 지속시간 설정, 
         * 흔들기 이전의 오브젝트의 위치 저장, 
         * 나무의 내구도 감소
         */
        #endregion


        // 0.4초 동안 오브젝트 흔들기
        while (fTime > 0)
        {
            fTime -= Time.deltaTime;         

            transform.position = ((Vector2)Random.insideUnitSphere * 0.1f) + vInitPos;
            yield return new WaitForFixedUpdate();
        }
        // 위치를 다시 처음 위치로
        transform.position = vInitPos;

        // 나무의 내구도가 0 이하라면, 나무를 쓰러뜨리는 코루틴을 실행시키고,
        // 실행시킨 코루틴이 종료되면 오브젝트를 비활성화한 뒤 코루틴을 종료시킨다. 
        if (fDurability <= 0)
        {
            yield return StartCoroutine(CR_chopDownTree());

            gameObject.SetActive(false);

            yield break;
        }
        doingChopTree = false;        
    }

    protected virtual IEnumerator CR_chopDownTree()
    {
        float alpha = 1;

        // Fade . . .
        while (sprite.color.a > 0)
        {
            alpha -= 0.02f;
            sprite.color = new Color(1, 1, 1, alpha);

            yield return new WaitForFixedUpdate();
        }

        yield break;
    }

    #region 설명 :
    /// <summary>
    /// 나무를 베고나서 드롭되는 아이템을 담는다.
    /// </summary>
    #endregion
    protected abstract void DropItem();

    #region 설명 :
    /// <summary>
    /// 오브젝트가 활성화 될 때 실행된다.(초기화)
    /// </summary>
    #endregion
    protected abstract void InitTree();
}
