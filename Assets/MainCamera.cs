using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : Singleton<MainCamera>
{
    private readonly Vector3 TracingOffset = new Vector3(0, 2, 0);

    [Header("Shaking Property")]
    [SerializeField] private AnimationCurve _ShakeCurve;

    private float _RestShakeTime;
    private float _ShakeTime;
    private float _ShakeForcePerFrame;

    [Header("Tracing Property")]
    [SerializeField] private Transform _Target;
    [SerializeField] private float _TracingSpeed;

    public void CameraShake(float time, float force)
    {
        float forcePerFrame = force / time;
        float ratio = 1f - Mathf.Min(_RestShakeTime / _ShakeTime, 1f);

        if (forcePerFrame > _ShakeForcePerFrame * _ShakeCurve.Evaluate(ratio))
        {
            _ShakeForcePerFrame = forcePerFrame;
            _RestShakeTime = _ShakeTime = time;
        }
        else
        {
            _ShakeForcePerFrame += forcePerFrame;
        }
    }

    private void LateUpdate()
    {
        if (_RestShakeTime > 0)
        {
            _RestShakeTime -= Time.deltaTime;

            float ratio = 1f - _RestShakeTime / _ShakeTime;
            transform.parent.position = 
                Random.onUnitSphere * _ShakeForcePerFrame * _ShakeCurve.Evaluate(ratio);

            if (_RestShakeTime <= 0f)
            {
                _RestShakeTime = _ShakeForcePerFrame = _ShakeTime = 0f;
            }
        }
        // ========== Tracing ========== //

        Vector2 target = _Target.position + TracingOffset;

        transform.localPosition = Vector2.Lerp(transform.localPosition, target, Time.deltaTime * _TracingSpeed);
        transform.Translate(0, 0, -10f, Space.Self);
        
        // ========== Tracing ========== //
    }
}
