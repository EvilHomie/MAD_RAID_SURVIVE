using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class MiniGun : WarmingWeapon
{
    [SerializeField] protected float _fireRate;
    float _nextTimeTofire = 0;

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
        GunPointsStartAnimation(_shootingCTS.Token).Forget();
    }

    protected override void OnStopShooting()
    {
        foreach (var point in _gunPoints) point.StopShoot();
        CoolingTask().Forget();
    }

    void OnWarmed()
    {
        ShootingTask(_shootingCTS.Token).Forget();
    }

    async UniTask GunPointsStartAnimation(CancellationToken shootCT)
    {
        int index = _lastGunPointIndex;
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

    protected override async UniTaskVoid ShootingTask(CancellationToken shootCT)
    {
        while (!shootCT.IsCancellationRequested && !_onDestroyCTS.IsCancellationRequested)
        {
            if (Time.time >= _nextTimeTofire)
            {
                _nextTimeTofire = Time.time + 1f / _fireRate;
                if (alternateShooting) NextGunPoint().Shoot();
                else foreach (var point in _gunPoints) point.Shoot();
            }
            await UniTask.Yield();
        }
    }
}
