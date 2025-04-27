using Cysharp.Threading.Tasks;
using UnityEngine;

public class PulseLaser : WarmingWeapon
{
    private void Awake()
    {
        _onWarmed += OnWarmed;
    }

    protected override void OnStartShooting()
    {
        WarmUpTask(_shootingCTS.Token).Forget();
        GunPointsStartAnimation(_shootingCTS.Token).Forget();
    }

    protected override void OnStopShooting()
    {
        CoolingTask().Forget();
    }

    void OnWarmed()
    {
        foreach (var point in _gunPoints) point.OnShoot();
    }
}
