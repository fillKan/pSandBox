using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public enum Tide
    {
        FOLLOWING_TIDE,
        FALL_TIDE
    }

    [Tooltip("파도의 시작을 밀물과 썰물 중 어느것으로 할 것인지를 설정합니다.")]
    public Tide StartTide;
    [Tooltip("파도침을 구현할 오브젝트를 설정합니다.")]
    public GameObject WavingObjct;

    [Tooltip("파도의 크기의 증감치를 설정합니다.")]
    public float IncreaseAmount;
    [Tooltip("파도의 최대 높이를 설정합니다.")]
    public float MaximumWaveHeight;
    [Tooltip("파도의 최소 높이를 설정합니다.")]
    public float MinimumWaveHeight;

    private float sumIncreaseAmount = 0;

    private void Start()
    {
        StartCoroutine(CR_update());
    }

    private IEnumerator CR_update()
    {
        while(WavingObjct)
        {
            if(StartTide.Equals(Tide.FOLLOWING_TIDE))
            {
                if (sumIncreaseAmount <= MaximumWaveHeight)
                {
                    sumIncreaseAmount += IncreaseAmount * Time.deltaTime;

                    WavingObjct.transform.Translate(0, IncreaseAmount * Time.deltaTime, 0);
                }
                else
                {
                    StartTide = Tide.FALL_TIDE;
                }
            }
            else
            {
                if (sumIncreaseAmount >= MinimumWaveHeight)
                {
                    sumIncreaseAmount -= IncreaseAmount * Time.deltaTime;

                    WavingObjct.transform.Translate(0, -IncreaseAmount * Time.deltaTime, 0);
                }
                else
                {
                    StartTide = Tide.FOLLOWING_TIDE;
                }
            }

            yield return null;
        }

        yield break;
    }
}
