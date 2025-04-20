using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class StandartWeapon : Weapon
{
    public float _nextTimeTofire;

    public override void OnStartShoot()
    {
        _shootCTS = _shootCTS.Create();
        StandartWeaponShootingTask(_shootCTS.Token).Forget();
    }

    public override void OnStopShoot()
    {
        _shootCTS.CancelAndDispose();
    }

    async UniTaskVoid StandartWeaponShootingTask(CancellationToken shootCT)
    {
        while (!shootCT.IsCancellationRequested)
        {
            if (Time.time >= _nextTimeTofire)
            {
                _shootEvent?.Invoke();
                _nextTimeTofire = Time.time + 1f / _fireRate;
                _gunpointAnimationService.StandartGunPointAnimation(_fireRate, GunPoinForNextShoot()).Forget();
            }
            await UniTask.Yield();
        }
    }
}
