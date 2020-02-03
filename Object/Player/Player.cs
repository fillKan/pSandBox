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
}

public class Player : MonoBehaviour
{
    private Vector2 vDir;
    private SpriteRenderer sprite;

    private Dictionary<int, Interaction> InteractObj = new Dictionary<int, Interaction>();

    public void AddInteractObj(int instanceID, Interaction interaction)
    {
        if(!InteractObj.ContainsKey(instanceID))
        {
            InteractObj.Add(instanceID, interaction);
        }
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
            #region 상호작용
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // 충돌한 collider를 모두 담을 RaycastHit2D배열.
                // collider를 모두 담는 이유는, collider를 가진 오브젝트들 모두가
                // *상호작용* 인터페이스를 보유하지는 않기 때문.
                RaycastHit2D[] hits;

                // 바라보고 있는 방향은 스프라이트의 flip값을 통해 확인.
                if (sprite.flipX)
                {
                    // *광선투사*
                    // 왼쪽 방향으로 0.75의 거리안에 위치한 모든 collider를 담는다~
                    hits = Physics2D.RaycastAll(transform.position, Vector2.left, 0.75f);
                    
                    // 광선 투사를 통해 감지한 collider의 숫자만큼 반복.
                    for (int i = 0; i < hits.Length; i++)
                    {
                        // 만약 광선투사를 통해 감지한 오브젝트가 상호작용 오브젝트라면?
                        if (InteractObj.ContainsKey(hits[i].collider.gameObject.GetInstanceID()))
                        {
                            // 상호작용 실행!
                            InteractObj[hits[i].collider.gameObject.GetInstanceID()].OperateAction();

                            break;
                        }
                    }
                }
                // 이하 동문
                else
                {
                    hits = Physics2D.RaycastAll(transform.position, Vector2.right, 0.75f);

                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (InteractObj.ContainsKey(hits[i].collider.gameObject.GetInstanceID()))
                        {
                            InteractObj[hits[i].collider.gameObject.GetInstanceID()].OperateAction();

                            break;
                        }
                    }
                }
                yield return StartCoroutine(CR_Vibration(0.1f, 0.3f));
            }
            #endregion

            if (Input.GetAxis("Horizontal") != 0)
            {
                vDir.x += Input.GetAxis("Horizontal") * Time.deltaTime * 3.5f;

                if (Input.GetAxis("Horizontal") > 0) sprite.flipX = false;
                if (Input.GetAxis("Horizontal") < 0) sprite.flipX = true;

                transform.position = vDir;
            }

            yield return new WaitForFixedUpdate();
        }

        yield break;
    }

    private IEnumerator CR_Vibration(float amount, float time)
    {
        Vector2 vInitPos = transform.position;

        while(time > 0)
        {
            time -= Time.deltaTime;

            transform.position = ((Vector2)Random.insideUnitSphere * amount) + vInitPos;

            yield return new WaitForFixedUpdate();
        }
        transform.position = vInitPos;

        yield break;
    }
}
