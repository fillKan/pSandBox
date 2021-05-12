﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    public float fMaxSpeed;
    public bool isHen;

    private float fMovementTimer = 0;
    private float         fTimer = 0;

    private SpriteRenderer sprite;

    private bool isMoving = false;

    #region 변수 설명 :
    /*
     * fMaxSpeed : 닭의 최대 속도를 지정하는 변수.
     * fMovementTimer    : 초 단위의 시간을 저장하는 변수. 
     * sprite    : 닭의 스프라이트를 담는 변수.
     * 
     */
    #endregion

    private void OnEnable()
    {
        sprite = GetComponent<SpriteRenderer>();

        StartCoroutine(CR_update());
    }

    private IEnumerator CR_update()
    {
        // fCoolTime은 다음 움직임까지 걸리는 시간을 저장한다.
        float fMoveCoolTime = Random.Range(0.2f, 1.6f);
        float fSpawnEggTime = Random.Range(50, 70);

        while(gameObject.activeSelf)
        {
            fMovementTimer += Time.deltaTime;

            if(isHen)
            {
                fTimer += Time.deltaTime;

                if (fTimer >= fSpawnEggTime)
                {
                    fTimer = 0;
                    ItemExisting item = ItemMaster.Instance.TakeItemExisting(ItemList.EGG);

                    item.transform.position = transform.position;
                    item.gameObject.SetActive(true);

                    fSpawnEggTime = Random.Range(50, 70);
                }
            }

            // 1.5초 이상의 시간이 지나면,
            if(fMovementTimer >= fMoveCoolTime && !isMoving)
            {
                // 시간을 0으로 초기화
                fMovementTimer = 0;

                // 2/3의 확률로 움직인다!
                if(Random.Range(0,3) != 2)
                {
                    // 움직임 코루틴 실행 (실행이 종료될 떄까지 대기)
                    isMoving = true;
                    StartCoroutine(CR_movement());
                }
                fMoveCoolTime = Random.Range(0.2f, 1.6f);
            }
            yield return null;
        }
        yield break;
    }

    private IEnumerator CR_movement()
    {
        Vector2 vTarget = transform.position;
                vTarget.x += Random.Range(-4.0f, 4.1f);

        // vTarget은 현재 위치를 기준으로 랜덤한 지점을 지정한다.(x축만)

        // vTarget가 지정한 곳이 지금의 위치라면, 해당 코루틴을 종료한다.
        if (vTarget.x.Equals(transform.position.x))
        {
            isMoving = false;
            yield break;
        }
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
        while((int)(transform.position.x * 100) != (int)(vTarget.x * 100))
        {
            // 부드럽게 움직이기. . . 최대 속도는 fMaxSpeed에 의해 제어된다.
            transform.position = Vector2.SmoothDamp(transform.position, vTarget, ref vRefVel, 0.7f, fMaxSpeed);

            // 스케일이 커져야해!
            if (growBiggerTurn)
            {
                if (vRefVel.x < 0)
                {
                     transform.localScale += new Vector3(0, Time.deltaTime * -vRefVel.x, 0);
                }
                else transform.localScale += new Vector3(0, Time.deltaTime *  vRefVel.x, 0);

                // 스케일의 변화량은 움직이는 속도에 맞춘다.
                // vRefVel.x의 값이 양수일 때 자연스럽기 떄문에 vRefVel.x이 양수인지 음수인지를 확인한다.

                // 스케일이 일정수준에 도달한다면? 작아지기로 한다.
                if (transform.localScale.y >= 1.15f) growBiggerTurn = false;
            }
            // 스케일이 작아져야해!
            else
            {
                if (vRefVel.x < 0)
                {
                     transform.localScale -= new Vector3(0, Time.deltaTime * -vRefVel.x, 0);
                }
                else transform.localScale -= new Vector3(0, Time.deltaTime *  vRefVel.x, 0);

                // 스케일의 변화량은 움직이는 속도에 맞춘다.
                // vRefVel.x의 값이 양수일 때 자연스럽기 떄문에 vRefVel.x이 양수인지 음수인지를 확인한다.

                // 스케일이 일정수준에 도달한다면? 커지기로 한다.
                if (transform.localScale.y <= 0.85f) growBiggerTurn = true;
            }
            yield return null;
        }

        // 움직임이 끝났다면, 스케일 원상복구

        vScale = transform.localScale;
        // 선형 보간을 통해 서서히 스케일을 복구한다.
        while (fTime < 1)
        {
            fTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(vScale, Vector3.one, fTime);
            
            yield return null;
        }

        // 움직임이 끝났고, 스케일이 복구되었다면 종료!
        isMoving = false;

        yield break;
    }
}