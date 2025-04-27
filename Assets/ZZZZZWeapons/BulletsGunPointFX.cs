using UnityEngine;

public class BulletsGunPointFX : AbstractGunPointFX
{
    [SerializeField] ParticleSystem _muzzleFlash;
    [SerializeField] ParticleSystem _bulletsParticles;
    [SerializeField] ParticleSystem _hitParticles;
    [SerializeField] ParticlesCollision _particlesCollision;

    private void Awake()
    {
        _particlesCollision._collision += OnHit;
    }

    private void OnHit(Vector3 vector)
    {
        _hitParticles.transform.position = vector;
        _hitParticles.Emit(5);
    }

    public override void OnShoot()
    {
        _muzzleFlash.Emit(1);
        _bulletsParticles.Emit(1);
        LightFlickerTask(_config.LightOnSingleShootFlickDuration).Forget();
    }

    
}
