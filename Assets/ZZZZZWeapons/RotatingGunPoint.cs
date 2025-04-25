using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class RotatingGunPoint : AbstractGunPoint
{
    [SerializeField] RotateDir _rotateDirection;
    Vector3 _direction;
    float _warmValue;

    public override void Init(Config config, CancellationToken onDestroyCTS)
    {
        base.Init(config, onDestroyCTS);
        _direction = _rotateDirection == RotateDir.Clockwise ? Vector3.back : Vector3.forward;
        _warmValue = 0;
    }

    public override void OnStartShooting(CancellationToken shootCT, float fireRate)
    {
        base.OnStartShooting(shootCT, fireRate);
        SpeedUpAnimation(shootCT).Forget();
    }
    public override void OnStopShooting()
    {
        base.OnStopShooting();
        SlowdownAnimation().Forget();
    }

    async UniTaskVoid SpeedUpAnimation(CancellationToken shootCT)
    {
        while (!shootCT.IsCancellationRequested && !_onDestroyCTS.IsCancellationRequested)
        {
            _warmValue += Time.deltaTime;
            _warmValue = Mathf.Clamp01(_warmValue);
            float rotateSpeed = _config.RotationAnimationCurve.Evaluate(_warmValue) * _fireRate;
            rotateSpeed = Mathf.Clamp(rotateSpeed, 0, _config.MaxRotationSpeed);
            transform.Rotate(_direction, rotateSpeed, Space.Self);
            await UniTask.Yield();
        }
    }

    async UniTaskVoid SlowdownAnimation()
    {
        while (!_inUse && !_onDestroyCTS.IsCancellationRequested)
        {
            _warmValue -= Time.deltaTime;
            _warmValue = Mathf.Clamp01(_warmValue);
            float rotateSpeed = _config.RotationAnimationCurve.Evaluate(_warmValue) * _fireRate;
            rotateSpeed = Mathf.Clamp(rotateSpeed, 0, _config.MaxRotationSpeed);
            transform.Rotate(_direction, rotateSpeed, Space.Self);
            if (_warmValue <= 0)
            {
                return;
            }

            await UniTask.Yield();
        }
    }
}
