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
                // 광선투사를 통한 충돌 정보를 담는 변수. 
                RaycastHit2D hit2D;

                // 바라보고 있는 방향은 스프라이트의 flip값을 통해 확인.
                if (sprite.flipX)
                {
                    // 특정 layer하고만 충돌한다! 레이어 마스크 설정!
                    // 이걸 시프트 연산을 하는 이유는..
                    // raycast함수에서는 비트마스크를 사용해 각각의 비트에서 각 레이어를 무시할지의 여부를 판정시키기 때문임.
                    int layerMask = 1 << LayerMask.NameToLayer("Player Interaction");

                    // Player Interaction 레이어 하고만 충돌하는 광선을 투사!
                    hit2D = Physics2D.Raycast(transform.position, Vector2.left, 0.75f, layerMask);

                    if (hit2D)
                    {
                        // 닿았나? 그렇다면 상호작용 실행!
                        InteractObj[hit2D.collider.gameObject.GetInstanceID()].OperateAction();
                    }
                }
                // 이하 동문
                else
                {
                    int layerMask = 1 << LayerMask.NameToLayer("Player Interaction");

                    hit2D = Physics2D.Raycast(transform.position, Vector2.right, 0.75f, layerMask);

                    if(hit2D)
                    {
                        InteractObj[hit2D.collider.gameObject.GetInstanceID()].OperateAction();
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
