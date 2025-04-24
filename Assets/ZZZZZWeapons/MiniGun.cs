using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class MiniGun : WarmingWeapon
{
    protected override void OnStartShooting()
    {
        WarmUpTask(_shootingCTS.Token, OnWarmed).Forget();
        GunPointsStartAnimation(_shootingCTS.Token).Forget();
    }

    protected override void OnStopShooting()
    {
        CoolingTask().Forget();
    }

    void OnWarmed()
    {
        ShootingTask(_shootingCTS.Token).Forget();
    }

    async UniTask GunPointsStartAnimation(CancellationToken shootCT)
    {
        int index = _nextGunPointForShootIndex;
        if (alternateShooting)
        {
            for (int i = 0; i < _gunPoints.Length; i++)
            {
                index++;
                if (index >= _gunPoints.Length) index = 0;
                _gunPoints[index].OnStartShooting(shootCT, _fireRate);
                await UniTask.Delay(TimeSpan.FromSeconds(1 / _fireRate), ignoreTimeScale: false, cancellationToken: shootCT);
            }
        }
        else foreach (var point in _gunPoints) point.OnStartShooting(shootCT, _fireRate);
    }
}
