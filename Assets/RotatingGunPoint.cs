using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class RotatingGunPoint : AbstractGunPoint
{
    [SerializeField] RotateDir _rotateDirection;
    bool _isShooting;
    Vector3 _direction;

    public override void Init(Config config, CancellationToken onDestroyCTS)
    {
        base.Init(config, onDestroyCTS);
        _isShooting = false;
        _direction = _rotateDirection == RotateDir.Clockwise ? Vector3.back : Vector3.forward;
    }


    public override void OnStartShoot(CancellationToken shootCT, float fireRate, float startAnimValue)
    {
        RotateAnimation(shootCT, fireRate, startAnimValue).Forget();
    }
    public async UniTaskVoid RotateAnimation(CancellationToken shootCT, float animSpeed, float animValue)
    {
        _isShooting = true;
        while (!shootCT.IsCancellationRequested && !_onDestroyCTS.IsCancellationRequested)
        {
            if (animValue < _config.WarmingTime) animValue += Time.deltaTime;

            animValue = Mathf.Clamp01(animValue);
            float rotateSpeed = _config.RotationAnimationCurve.Evaluate(animValue) * animSpeed;
            transform.Rotate(_direction, rotateSpeed, Space.Self);
            await UniTask.Yield();
        }

        _isShooting = false;
        while (!_isShooting && !_onDestroyCTS.IsCancellationRequested)
        {
            animValue -= Time.deltaTime;
            animValue = Mathf.Clamp01(animValue);
            float rotateSpeed = _config.RotationAnimationCurve.Evaluate(animValue) * animSpeed;
            transform.Rotate(_direction, rotateSpeed, Space.Self);
            if (animValue <= 0)
            {
                break;
            }

            await UniTask.Yield();
        }
    }
}
