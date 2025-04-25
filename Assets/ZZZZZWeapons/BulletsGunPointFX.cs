using UnityEngine;

public class BulletsGunPointFX : AbstractGunPointFX
{
    [SerializeField] ParticleSystem _muzzleFlash;
    [SerializeField] ParticleSystem _bulletsParticles;

    public override void OnShoot()
    {
        _muzzleFlash.Emit(1);
        _bulletsParticles.Emit(1);
        LightFlickerTask().Forget();
    }
}
