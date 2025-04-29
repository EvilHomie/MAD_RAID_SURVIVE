using System.Threading;
using UnityEngine;

public class BulletsGunPointFX : AbstractGunPointFX
{
    [SerializeField] ParticleSystem _muzzleFlash;
    [SerializeField] ParticleSystem _bulletsParticles;
    [SerializeField] ParticleSystem _hitParticles;

    

    public override void OnInit()
    {
    }

    public override void OnStartShooting(CancellationToken shootCT, float fireRate = 0)
    {
    }

    public override void Shoot()
    {
        _muzzleFlash.Emit(1);
        _bulletsParticles.Emit(1);
        LightFlickerTask(_config.LightOnSingleShootFlickDuration).Forget();
    }

    public override void StopShoot()
    {
    }

    public override void OnHit(GameObject hitedObj, Vector3 pos)
    {
        _hitParticles.transform.position = pos;
        _hitParticles.Emit(_config.ParticlesCountOnBulletCollision);
    }
}
