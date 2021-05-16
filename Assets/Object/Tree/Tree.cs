using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : InteractableObject
{
    [Header("Tree Property")]
    [SerializeField] private float _Durability;
    [SerializeField] private ItemDropper _ItemDropper;
    [SerializeField] private Collider2D _Collider;

    [SerializeField] private Animator _Animator;
    private int _AnimControlKey;

    [Header("Shaking Property")]
    [SerializeField] private AnimationCurve _ShakingCurve;

    private float _RestShakeTime = 0f;
    private float _ShakeTime = 0f;
    private float _ShakeForcePerFrame = 0f;

    private float _RestDurability;

    public override void OnActive()
    {
        base.OnActive();

        StartCoroutine(ShakingRoutine());
        _AnimControlKey = _Animator.GetParameter(0).nameHash;

        _RestDurability = _Durability;
        _Collider.enabled = true;
    }
    public override void Interaction()
    {
        Shaking(0.3f, 0.05f);

        if ((_RestDurability -= StateStorage.Instance.TreeLogging) <= 0)
        {
            _ItemDropper.DropItem(0);

            InteractionManager.Instance.DisRegister(gameObject.GetInstanceID());
            _Collider.enabled = false;
            _Animator.SetInteger(_AnimControlKey, 1);

            CursorPointer.Instance.HighLightRelease();
        }
    }
    private void Shaking(float time, float force)
    {
        float forcePerFrame = force / time;
        float ratio = 1f - Mathf.Min(_RestShakeTime / _ShakeTime, 1f);

        if (forcePerFrame > _ShakeForcePerFrame * _ShakingCurve.Evaluate(ratio))
        {
            _ShakeForcePerFrame = forcePerFrame;
            _RestShakeTime = _ShakeTime = time;
        }
        else
        {
            _ShakeForcePerFrame += forcePerFrame;
        }
    }
    private IEnumerator ShakingRoutine()
    {
        while (gameObject.activeInHierarchy) 
        {
            if (_RestShakeTime > 0)
            {
                _RestShakeTime -= Time.deltaTime;

                float ratio = 1f - _RestShakeTime / _ShakeTime;
                
                _Renderer.transform.localPosition 
                    = Random.insideUnitCircle * _ShakeForcePerFrame * _ShakingCurve.Evaluate(ratio);

                if (_RestShakeTime <= 0f) {
                    _RestShakeTime = _ShakeForcePerFrame = _ShakeTime = 0f;
                }
            }
            yield return null;
        }
    }

    private void AE_FadeCompleted()
    {
        gameObject.SetActive(false);
    }
}
