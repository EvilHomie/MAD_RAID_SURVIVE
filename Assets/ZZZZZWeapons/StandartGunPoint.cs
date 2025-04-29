using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class StandartGunPoint : AbstractGunPoint
{
    [SerializeField] ProjectileParticlesCollision _projectileParticlesCollision;
    Vector3 _deffPos;
    float _fireRate;

    public override void OnInit()
    {
        _deffPos = transform.localPosition;
        _projectileParticlesCollision._onCollisionWithObject += OnHit;
    }

    private void OnDisable()
    {
        _projectileParticlesCollision._onCollisionWithObject -= OnHit;
    }

    public override void OnStartShooting(CancellationToken shootCT, float fireRate = 0)
    {
        _fireRate = fireRate;
    }

    public override void Shoot()
    {
        abstractShootVFX.Shoot();
        ShootAnimation(_fireRate).Forget();
    }

    public async UniTaskVoid ShootAnimation(float fireRate)
    {
        float t = 0;
        while (t < 1 && !_onDestroyCTS.IsCancellationRequested)
        {
            t += Time.deltaTime * fireRate;
            t = Mathf.Clamp01(t);
            float zOffset = _config.ForwardMovementAnimationCurve.Evaluate(t);
            transform.localPosition = _deffPos + Vector3.forward * zOffset;
            await UniTask.Yield();
        }
    }

    public override void StopShoot()
    {
    }
}
