using UnityEngine;

public class BulletsGunPointFX : AbstractGunPointFX
{
    [SerializeField] ParticleSystem _muzzleFlash;
    [SerializeField] ParticleSystem _bulletsParticles;

    public override void OnShoot()
    {
        _muzzleFlash.Emit(1);
        _bulletsParticles.Emit(1);
        LoghtFlickerTask().Forget();


        //if (!_isShotting)
        //{
        //    _isShotting = true;
        //    Flicker(_shootCT).Forget();
        //    SetAndEnableEffects(_muzzleFlash);
        //    SetAndEnableEffects(_bulletsParticles);
        //}
    }

    //void SetAndEnableEffects(ParticleSystem particleSystem)
    //{
    //    particleSystem.Play();
    //    var emision = particleSystem.emission;
    //    emision.rateOverTime = _fireRate;
    //    emision.enabled = true;
    //}

    //void DisableEmision(ParticleSystem particleSystem)
    //{
    //    var emision = particleSystem.emission;
    //    emision.enabled = false;
    //}
}
