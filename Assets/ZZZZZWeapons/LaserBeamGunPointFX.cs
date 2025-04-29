using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class LaserBeamGunPointFX : AbstractGunPointFX
{
    [SerializeField] ParticleSystem _chargePointParticles;
    [SerializeField] ParticleSystem _chargeFloatingParticles;
    [SerializeField] ParticleSystem _shootParticlesBurst;
    [SerializeField] ParticleSystem _shootParticlesContinuously;
    [SerializeField] ParticleSystem _hitParticles;
    [SerializeField] LineRenderer _laserBeam;
    float _warmValue;

    bool _isShooting;
    bool _inUse;

    public override void OnInit()
    {
        _laserBeam.enabled = false;
        _isShooting = false;
        _inUse = false;
        _hitParticles.Stop();
    }
    public override void OnStartShooting(CancellationToken shootCT, float fireRate = 0)
    {
        _inUse = true;
        _light.enabled = true;
        WarmUpTask(shootCT).Forget();
    }
    public override void Shoot()
    {
        if (!_isShooting)
        {
            _isShooting = true;
            _laserBeam.enabled = true;
            _hitParticles.Play();
            //ActiveLaserBeam().Forget();
        }
    }
    public override void StopShoot()
    {
        _inUse = false;
        _isShooting = false;
        _laserBeam.enabled = false;
        _hitParticles.Stop();
        _hitParticles.Clear();
        CoolingTask().Forget();
    }

    public override void OnHit(GameObject hitedObj, Vector3 pos)
    {
        _laserBeam.SetPosition(0, _laserBeam.transform.position);
        _laserBeam.SetPosition(1, pos);
        _hitParticles.transform.position = pos;
        _hitParticles.Play();
    }

    async UniTaskVoid WarmUpTask(CancellationToken shootCT)
    {        
        _chargePointParticles.Play();
        _chargeFloatingParticles.Play();
        while (!shootCT.IsCancellationRequested && !_onDestroyCTS.IsCancellationRequested)
        {
            _warmValue += Time.deltaTime;
            _warmValue = Mathf.Clamp01(_warmValue);
            _light.intensity = _warmValue / 2;
            _chargePointParticles.transform.localScale = Vector3.one * _warmValue;

            if (_warmValue == 1)
            {
                _chargeFloatingParticles.Stop();
                _shootParticlesBurst.Emit(5);
                _shootParticlesContinuously.Play();
                LightIntensityFlickerOnShooting().Forget();
                return;
            }
            await UniTask.Yield();
        }
    }

    async UniTaskVoid CoolingTask()
    {        
        _shootParticlesContinuously.Stop();
        _chargeFloatingParticles.Stop();
        while (!_inUse && !_onDestroyCTS.IsCancellationRequested)
        {
            _warmValue -= Time.deltaTime;
            _warmValue = Mathf.Clamp01(_warmValue);
            _light.intensity = _warmValue / 2;
            _chargePointParticles.transform.localScale = Vector3.one * _warmValue;
            if (_warmValue == 0)
            {
                _chargePointParticles.Stop();
                return;
            }
            await UniTask.Yield();
        }
    }


    async UniTaskVoid LightIntensityFlickerOnShooting()
    {
        while (_inUse && !_onDestroyCTS.IsCancellationRequested)
        {
            _light.intensity = 0.9f;
            _chargePointParticles.transform.localScale = Vector3.one * _light.intensity;
            await UniTask.Delay(TimeSpan.FromSeconds(_config.LightOnContinuouslyShootingFlickDuration), ignoreTimeScale: false, cancellationToken: _onDestroyCTS);
            _light.intensity = 1f;
            _chargePointParticles.transform.localScale = Vector3.one * _light.intensity;
            await UniTask.Delay(TimeSpan.FromSeconds(_config.LightOnContinuouslyShootingFlickDuration), ignoreTimeScale: false, cancellationToken: _onDestroyCTS);
        }
    }

    //async UniTaskVoid ActiveLaserBeam()
    //{
    //    _laserBeam.enabled = true;
    //    while (_isShooting && !_onDestroyCTS.IsCancellationRequested)
    //    {
    //        if (Physics.Raycast(_laserBeam.transform.position, _laserBeam.transform.forward, out RaycastHit firePointhitInfo, 1000000))
    //        {
    //            _laserBeam.SetPosition(0, _laserBeam.transform.position);
    //            _laserBeam.SetPosition(1, firePointhitInfo.point);
    //            _hitParticles.transform.position = firePointhitInfo.point;
    //            _hitParticles.Play();
    //        }
    //        await UniTask.Yield();
    //    }
    //}

   
}
