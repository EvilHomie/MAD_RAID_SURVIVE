using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class StandartCanon : AbstractWeapon
{
    [SerializeField] protected float _fireRate;
    float _nextTimeTofire = 0;

    protected override async UniTaskVoid ShootingTask(CancellationToken shootingCT)
    {
        foreach (var point in _gunPoints) point.OnStartShooting(shootingCT, _fireRate);

        while (!shootingCT.IsCancellationRequested && !_onDestroyCTS.IsCancellationRequested)
        {
            if (Time.time >= _nextTimeTofire)
            {
                _nextTimeTofire = Time.time + 1f / _fireRate;
                if (_alternateShooting) NextGunPoint().Shoot();
                else foreach (var point in _gunPoints) point.Shoot();
            }
            await UniTask.Yield();
        }
    }

    protected override void OnStartShooting()
    {
        ShootingTask(_shootingCTS.Token).Forget();
    }

    protected override void OnStopShooting()
    {
    }

    protected override void OnHitGameObject(GameObject hitedObject, Vector3 hitPos)
    {
        _hitcallback(hitedObject, hitPos, _damage);
    }
}
