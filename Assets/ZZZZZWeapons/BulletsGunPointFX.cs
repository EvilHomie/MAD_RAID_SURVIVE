using UnityEngine;

public class BulletsGunPointFX : AbstractGunPointFX
{
    [SerializeField] ParticleSystem _muzzleFlash;

    public override void OnShoot()
    {
        _muzzleFlash.Emit(1);
    }
}
