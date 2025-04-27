using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine;

public class PulseLaserGunPointFX : AbstractGunPointFX
{
    [SerializeField] ParticleSystem _chargePointParticles;
    [SerializeField] ParticleSystem _chargeFloatingParticles;
    [SerializeField] ParticleSystem _shootParticlesBurst;
    [SerializeField] ParticleSystem _shootParticlesContinuously;
    [SerializeField] LineRenderer _laserBeam;
    [SerializeField] float _beamFlickerDuration;
    float _warmValue;

    bool _isShooting;

    public override void Init(Config config, CancellationToken onDestroyCTS)
    {
        base.Init(config, onDestroyCTS);
        _laserBeam.enabled = false;
        _isShooting = false;
    }

    public override void OnStartShooting(CancellationToken shootCT, float fireRate)
    {
        base.OnStartShooting(shootCT, fireRate);
        WarmUpTask(shootCT).Forget();
        _light.enabled = true;
    }
    public override void OnStopShooting()
    {
        base.OnStopShooting();
        CoolingTask().Forget();
        _isShooting = false;
        _laserBeam.enabled = false;
    }
    public override void OnShoot()
    {
        base.OnShoot();
        if (!_isShooting)
        {
            _isShooting = true;
            ActiveLaserBeam().Forget();
            PulseAnimation().Forget();
        }

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

    async UniTaskVoid PulseAnimation()
    {
        while (_inUse && !_onDestroyCTS.IsCancellationRequested)
        {
            _laserBeam.enabled = true;
            await UniTask.Delay(TimeSpan.FromSeconds(_beamFlickerDuration), ignoreTimeScale: false, cancellationToken: _onDestroyCTS);
            _laserBeam.enabled = false;
            await UniTask.Delay(TimeSpan.FromSeconds(_beamFlickerDuration), ignoreTimeScale: false, cancellationToken: _onDestroyCTS);
        }
    }

    async UniTaskVoid ActiveLaserBeam()
    {
        while (_isShooting && !_onDestroyCTS.IsCancellationRequested)
        {
            if (Physics.Raycast(_laserBeam.transform.position, _laserBeam.transform.forward, out RaycastHit firePointhitInfo, 1000000))
            {
                _laserBeam.SetPosition(0, _laserBeam.transform.position);
                _laserBeam.SetPosition(1, firePointhitInfo.point);
            }


            await UniTask.Yield();
        }
    }
}
