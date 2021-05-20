using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    private const string FishingColliderName = "Water";

    [SerializeField] private Rigidbody2D _Rigidbody;
    [SerializeField] private float _WaitTimeMin;
    [SerializeField] private float _WaitTimeMax;
    [SerializeField] private Animator _Animator;
    private int _AinmControlKey;

    public Rigidbody2D Rigidbody => _Rigidbody;

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
            _Animator.SetBool(_AinmControlKey, true);
        }
    }
    public void CatchFish()
    {
        if (_Animator.GetBool(_AinmControlKey))
        {
            ItemName fish = ItemMaster.Instance.RandomFish();

            _CatchedItem = ItemMaster.Instance.GetDroppedItem(fish);
            _CatchedItem.Rigidbody.isKinematic = true;
            _CatchedItem.transform.SetParent(transform);
            _CatchedItem.transform.localPosition = Vector3.zero;
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
        _Animator.SetBool(_AinmControlKey, false);
    }
    private void OnEnable()
    {
        _AinmControlKey = _Animator.GetParameter(0).nameHash;
    }
    private void AE_NiddleOver()
    {
        _Animator.SetBool(_AinmControlKey, false);
    }
}
