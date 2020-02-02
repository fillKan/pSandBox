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
    /// 고정된 형식 사용 : PlayerGetter.Instance.AddInteractObj(this);
    /// </summary>
    #endregion
    void RegisterInteraction();
}

public class Player : MonoBehaviour
{
    private Vector2 vDir;
    private SpriteRenderer sprite;

    private LinkedList<Interaction> InteractObj = new LinkedList<Interaction>();

    public void AddInteractObj(Interaction interaction)
    {
        InteractObj.AddLast(interaction);
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RaycastHit2D hit2D;

                if (sprite.flipX)
                {
                    hit2D = Physics2D.Raycast(transform.position, Vector2.left, 0.75f);

                    if (hit2D)
                    {
                        InteractObj.Find(hit2D.collider.GetComponent<Interaction>()).Value.OperateAction();
                    }
                }
                else
                {
                    hit2D = Physics2D.Raycast(transform.position, Vector2.right, 0.75f);

                    if (hit2D)
                    {
                        InteractObj.Find(hit2D.collider.GetComponent<Interaction>()).Value.OperateAction();
                    }
                }
                yield return StartCoroutine(CR_Vibration(0.1f, 0.3f));
            }
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
