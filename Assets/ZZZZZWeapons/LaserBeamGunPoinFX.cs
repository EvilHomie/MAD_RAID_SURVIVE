using System.Threading;
using UnityEngine;

public class LaserBeamGunPoinFX : AbstractGunPointFX
{
    bool _isShoting;

    public override void OnStartShooting(CancellationToken shootCT, float fireRate)
    {
        base.OnStartShooting(shootCT, fireRate);
    }
    public override void OnStopShooting()
    {
        base.OnStopShooting();
    }
    public override void OnShoot()
    {
        base.OnShoot();

        if (!_isShoting)
        {
            _isShoting = true;
        }
    }
}
