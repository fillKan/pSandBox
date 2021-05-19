using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    private const string FishingColliderName = "Water";

    [SerializeField] private Rigidbody2D _Rigidbody;

    [Header("Fishing Property")]
    [SerializeField] private float _WaitTimeMin;
    [SerializeField] private float _WaitTimeMax;

    [Space()] [SerializeField] private float _OnFishTime;

    public Rigidbody2D Rigidbody => _Rigidbody;

    private bool _CanCatching;
    private IEnumerator _WaitFishRoutine;
    private DroppedItem _CatchedItem;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(FishingColliderName))
        {
            if (_WaitFishRoutine == null && _Rigidbody.velocity.sqrMagnitude < 1.0f)
            {
                StartCoroutine(_WaitFishRoutine = WaitFishRoutine());
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(FishingColliderName))
        {
            if (_WaitFishRoutine != null)
            {
                StopCoroutine(_WaitFishRoutine);
                _WaitFishRoutine = null;
            }
        }
    }
    private IEnumerator WaitFishRoutine()
    {
        while (gameObject.activeSelf)
        {
            float waitTime = Random.Range(_WaitTimeMin, _WaitTimeMax);

            for (float i = 0; i < waitTime; i += Time.deltaTime * Time.timeScale)
                yield return null;

            Vector2 startPos = transform.position;
            _CanCatching = true;
            for (float i = 0; i < _OnFishTime; i += Time.deltaTime)
            {
                transform.position = startPos + (Random.insideUnitCircle * 0.05f);
                yield return null;
            }
            transform.position = startPos;
            _CanCatching = false;
        }
    }
    public void CatchFish()
    {
        if (_CanCatching)
        {
            ItemName fish = ItemMaster.Instance.RandomFish();

            _CatchedItem = ItemMaster.Instance.GetDroppedItem(fish);
            _CatchedItem.Rigidbody.isKinematic = true;
            _CatchedItem.transform.SetParent(transform);
            _CatchedItem.transform.localPosition = Vector3.zero;

            _CanCatching = false;
        }
    }
    public void DropFish()
    {
        if (_CatchedItem != null)
        {
            _CatchedItem.transform.SetParent(null);
            _CatchedItem.Rigidbody.isKinematic = false;
            _CatchedItem = null;
        }
    }
    private void OnDisable()
    {
        if (_WaitFishRoutine != null)
        {
            StopCoroutine(_WaitFishRoutine);
            _WaitFishRoutine = null;
        }
    }
}
