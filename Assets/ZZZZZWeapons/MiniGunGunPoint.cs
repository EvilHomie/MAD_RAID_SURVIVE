using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class MiniGunGunPoint : AbstractGunPoint
{
    [SerializeField] RotateDir _rotateDirection;
    [SerializeField] Transform _rotatingPart;
    [SerializeField] ProjectileParticlesCollision _projectileParticlesCollision;
    Vector3 _direction;
    float _warmValue;
    float _fireRate;
    bool _inUse;

    public override void OnInit()
    {
        _warmValue = 0;
        _direction = _rotateDirection == RotateDir.Clockwise ? Vector3.back : Vector3.forward;
        _projectileParticlesCollision._onCollisionWithObject += OnHit;
    }
    private void OnDisable()
    {
        _projectileParticlesCollision._onCollisionWithObject -= OnHit;
    }

    public override void OnStartShooting(CancellationToken shootCT, float fireRate = 0)
    {
        _fireRate = fireRate;
        _inUse = true;
        SpeedUpAnimation(shootCT).Forget();
    }

    public override void Shoot()
    {
        abstractShootVFX.Shoot();
    }

    public override void StopShoot()
    {
        _inUse = false;
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
            _rotatingPart.Rotate(_direction, rotateSpeed, Space.Self);
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
            _rotatingPart.Rotate(_direction, rotateSpeed, Space.Self);
            if (_warmValue <= 0)
            {
                return;
            }

            await UniTask.Yield();
        }
    }
}
