using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class LaserBeam : WarmingWeapon
{
    private void OnEnable()
    {
        _onWarmed += OnWarmed;
    }
    private void OnDisable()
    {
        _onWarmed -= OnWarmed;
    }

    protected override void OnStartShooting()
    {
        WarmUpTask(_shootingCTS.Token).Forget();
        GunPointsStartHeatAnimation(_shootingCTS.Token).Forget();
    }

    protected override void OnStopShooting()
    {
        //foreach (var point in _gunPoints) point.StopShoot();
        GunPointsStartDisableAnimation().Forget();
        CoolingTask().Forget();
    }

    void OnWarmed()
    {
        ShootingTask(_shootingCTS.Token).Forget();
    }

    async UniTask GunPointsStartHeatAnimation(CancellationToken shootCT)
    {
        int index = _lastGunPointIndex;
        if (alternateShooting)
        {
            for (int i = 0; i < _gunPoints.Length; i++)
            {
                index++;
                if (index >= _gunPoints.Length) index = 0;
                _gunPoints[index].OnStartShooting(shootCT);
                await UniTask.Delay(TimeSpan.FromSeconds(_config.WarmingTime / 2), ignoreTimeScale: false, cancellationToken: shootCT);
            }
        }
        else foreach (var point in _gunPoints) point.OnStartShooting(shootCT);
    }

    protected override async UniTaskVoid ShootingTask(CancellationToken shootCT)
    {
        
        if (alternateShooting)
        {
            for (int i = 0; i < _gunPoints.Length; i++)
            {
                NextGunPoint().Shoot();
                await UniTask.Delay(TimeSpan.FromSeconds(_config.WarmingTime / 2), ignoreTimeScale: false, cancellationToken: shootCT);
            }
        }
        else foreach (var point in _gunPoints) point.Shoot();
    }

    async UniTask GunPointsStartDisableAnimation()
    {
        int index = _lastGunPointIndex;
        if (alternateShooting)
        {
            for (int i = 0; i < _gunPoints.Length; i++)
            {
                //index++;
                //if (index >= _gunPoints.Length) index = 0;
                //_gunPoints[index].StopShoot();
                NextGunPoint().StopShoot();
                await UniTask.Delay(TimeSpan.FromSeconds(_config.WarmingTime / 2), ignoreTimeScale: false);
            }
        }
        else foreach (var point in _gunPoints) point.StopShoot();
    }

}
