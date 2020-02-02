﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public float fMaxSpeed;
    public Sprite sheepSprite;
    public Sprite woolySprite;

    private RECT rect;
    private float fTimer = 0;
    private SpriteRenderer sprite;

    #region 변수 설명 :
    /*
     * fMaxSpeed   : 양의 최대 속도를 지정하는 변수.
     * sheepSprite : 털이 깎인 상태의 스프라이트를 저정하는 변수
     * woolySprite : 털이 있는 상태의 스프라이트를 저장하는 변수
     * rect        : 플레이어와의 상호작용 범위 렉트
     * fTimer      : 초 단위의 시간을 저장하는 변수. 
     * sprite      : 양의 스프라이트를 담는 변수.
     * 
     */
    #endregion

    private void OnEnable()
    {
        sprite = GetComponent<SpriteRenderer>();
        rect.SetRect(1, -1, 0.75f, -0.75f);
        StartCoroutine(CR_update());
    }

    private IEnumerator CR_update()
    {
        // fCoolTime은 다음 움직임까지 걸리는 시간을 저장한다.
        float fCoolTime = Random.Range(0.2f, 1.6f);

        while (gameObject.activeSelf)
        {
            fTimer += Time.deltaTime;

            if (PlayerGetter.Instance.GetPos().x - transform.position.x <= rect.right
            && PlayerGetter.Instance.GetPos().x - transform.position.x >= rect.left
            && PlayerGetter.Instance.GetPos().y - transform.position.y <= rect.top
            && PlayerGetter.Instance.GetPos().y - transform.position.y >= rect.bottom)
            {
                if (Input.GetKeyDown(KeyCode.Space) && sprite.sprite == woolySprite)
                {
                    for(int i = 0; i < 5; i++)
                    {
                        Instantiate(ItemMaster.Instance.GetItem(ItemMaster.ItemList.WOOL), transform.position, Quaternion.identity);
                    }
                    sprite.sprite = sheepSprite;
                }
            }

            // 1.5초 이상의 시간이 지나면,
            if (fTimer >= fCoolTime)
            {
                // 시간을 0으로 초기화
                fTimer = 0;

                // 2/3의 확률로 움직인다!
                if (Random.Range(0, 3) != 2)
                {
                    // 움직임 코루틴 실행 (실행이 종료될 떄까지 대기)
                    yield return StartCoroutine(CR_movement());
                }
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }

    private IEnumerator CR_movement()
    {
        Vector2 vTarget = transform.position;
        vTarget.x += Random.Range(-4.0f, 4.1f);

        // vTarget은 현재 위치를 기준으로 랜덤한 지점을 지정한다.(x축만)

        // vTarget가 지정한 곳이 지금의 위치라면, 해당 코루틴을 종료한다.
        if (vTarget.x.Equals(transform.position.x)) yield break;

        // vRefVel은 움직임의 속도를 저장하며, 초기 속도는 0이다.
        // vScale은 움직임이 끝난뒤에 오브젝트의 크기를 저장한다.

        Vector2 vRefVel = Vector2.zero;
        Vector3 vScale;

        // growBiggerTurn은 지금 오브젝트의 스케일이 커져야할 상태인지, 아닌지를 저장한다.
        bool growBiggerTurn = true;
        float fTime = 0;

        // 움직이려는 방향에 맞춰 스프라이트 플립. . .
        if (vTarget.x > transform.position.x) sprite.flipX =  true;
        else                                  sprite.flipX = false;

        // 목표위치에 대강 도달할 때까지 반복
        while ((int)(transform.position.x * 100) != (int)(vTarget.x * 100))
        {
            // 부드럽게 움직이기. . . 최대 속도는 fMaxSpeed에 의해 제어된다.
            transform.position = Vector2.SmoothDamp(transform.position, vTarget, ref vRefVel, 0.7f, fMaxSpeed);

            // 스케일이 커져야해!
            if (growBiggerTurn)
            {
                if (vRefVel.x < 0)
                {
                     transform.localScale += new Vector3(0, Time.deltaTime * -vRefVel.x * 0.7f, 0);
                }
                else transform.localScale += new Vector3(0, Time.deltaTime *  vRefVel.x * 0.7f, 0);

                // 스케일의 변화량은 움직이는 속도에 맞춘다.
                // vRefVel.x의 값이 양수일 때 자연스럽기 떄문에 vRefVel.x이 양수인지 음수인지를 확인한다.

                // 스케일이 일정수준에 도달한다면? 작아지기로 한다.
                if (transform.localScale.y >= 1.05f) growBiggerTurn = false;
            }
            // 스케일이 작아져야해!
            else
            {
                if (vRefVel.x < 0)
                {
                     transform.localScale -= new Vector3(0, Time.deltaTime * -vRefVel.x * 0.7f, 0);
                }
                else transform.localScale -= new Vector3(0, Time.deltaTime *  vRefVel.x * 0.7f, 0);

                // 스케일의 변화량은 움직이는 속도에 맞춘다.
                // vRefVel.x의 값이 양수일 때 자연스럽기 떄문에 vRefVel.x이 양수인지 음수인지를 확인한다.

                // 스케일이 일정수준에 도달한다면? 커지기로 한다.
                if (transform.localScale.y <= 0.95f) growBiggerTurn = true;
            }

            yield return new WaitForFixedUpdate();
        }

        // 움직임이 끝났다면, 스케일 원상복구

        vScale = transform.localScale;
        // 선형 보간을 통해 서서히 스케일을 복구한다.
        while (fTime < 1)
        {
            fTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(vScale, Vector3.one, fTime);

            yield return new WaitForFixedUpdate();
        }

        // 움직임이 끝났고, 스케일이 복구되었다면 종료!
        yield break;
    }
}