using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class InstantShootWeapon : AbstractWeapon
{
    float _nextTimeTofire;

    public override void StartShoot()
    {
        base.StartShoot();
        ShootingTask(_shootingCTS.Token).Forget();
    }

    async UniTaskVoid ShootingTask(CancellationToken shootCT)
    {
        while (!shootCT.IsCancellationRequested)
        {
            if (Time.time >= _nextTimeTofire)
            {
                _nextTimeTofire = Time.time + 1f / _fireRate;
                OnStartShoot(0);
            }
            await UniTask.Yield();
        }
    }
}
