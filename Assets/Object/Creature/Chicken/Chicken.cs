using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    #region Animation Transition
    private const int Idle = 0;
    private const int Move = 1;
    #endregion

    private readonly Vector3 LookAtLeftVector = new Vector3(1, 1, 1);
    private readonly Vector3 LookAtRightVector = new Vector3(-1, 1, 1);

    [Header("# Sprite Property")]
    [SerializeField] private Animator _Animator;
    private int _AnimControlKey;

    [Header("# Chicken Property")]
    public bool IsHen;
    [SerializeField] private float _EggTimeMin;
    [SerializeField] private float _EggTimeMax;

    [Header("# Movement Property")]
    [SerializeField] private float _MoveSpeed;

    [Space()]
    [SerializeField] private float _MovingTimeMin;
    [SerializeField] private float _MovingTimeMax;

    [Space()]
    [SerializeField] private float _WaitingTimeMin;
    [SerializeField] private float _WaitingTimeMax;

    private IEnumerator _MovementRoutine;

    private void OnEnable()
    {
        _AnimControlKey = _Animator.GetParameter(0).nameHash;

        StartCoroutine(MovementPeriod());
        if (IsHen)
        {
            StartCoroutine(EggDropRoutine());
        }
    }

    private IEnumerator MovementPeriod()
    {
        while (gameObject.activeInHierarchy)
        {
            float waiting = Random.Range(_WaitingTimeMin, _WaitingTimeMax);

            for (float i = 0f; i < waiting; i += Time.deltaTime * Time.timeScale)
                yield return null;

            _Animator.SetInteger(_AnimControlKey, Move);
            yield return StartCoroutine(_MovementRoutine = MovementRoutine());
        }
    }
    private IEnumerator MovementRoutine()
    {
        float moving = Random.Range(_MovingTimeMin, _MovingTimeMax);
        Vector3 movingDir;

        if (Random.value > 0.5f)
        {
            movingDir = Vector3.left;
            transform.localScale = LookAtLeftVector;
        }
        else
        {
            movingDir = Vector3.right;
            transform.localScale = LookAtRightVector;
        }
        float deltaTime = Time.deltaTime * Time.timeScale;

        for (float i = 0f; i < moving; i += deltaTime)
        {
            transform.position += movingDir * deltaTime * _MoveSpeed;
            yield return null;

            deltaTime = Time.deltaTime * Time.timeScale;
        }
        _MovementRoutine = null;
        _Animator.SetInteger(_AnimControlKey, Idle);
    }
    private IEnumerator EggDropRoutine()
    {
        while (gameObject.activeInHierarchy)
        {
            float wait = Random.Range(_EggTimeMin, _EggTimeMax);
            
            for (float i = 0f; i < wait; i += Time.deltaTime * Time.timeScale) 
                yield return null;

            var egg = ItemMaster.Instance.GetItemExisting(ItemList.EGG);
            
            egg.transform.position = transform.position;
            egg.gameObject.SetActive(true);
        }
    }
}
