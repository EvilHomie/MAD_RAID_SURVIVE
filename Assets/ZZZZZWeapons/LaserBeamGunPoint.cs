using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class LaserBeamGunPoint : AbstractGunPoint
{
    [SerializeField] Renderer[] _heatBulbs;
    [SerializeField] Renderer[] _heatLines;
    [SerializeField] LayerMask _layerMaskForHit;

    int _fillingValuePropertyID;
    int _lerpValuePropertyID;
    float _warmValue;

    float _bulbsStep;
    Renderer _activeBulb;
    int _activeBulbIndex;
    float _activeBulbMaxBorder;
    float _activeBulbMinBorder;

    bool _inUse;

    public override void OnInit()
    {
        _warmValue = 0;
        _inUse = false;
        _fillingValuePropertyID = Shader.PropertyToID("_FillingValue");
        _lerpValuePropertyID = Shader.PropertyToID("_LerpValue");
        UpdateHeatLines();

        foreach (Renderer bulb in _heatBulbs)
        {
            bulb.material.SetFloat(_lerpValuePropertyID, 0);
        }

        _bulbsStep = 1f / _heatBulbs.Length;
        _activeBulbIndex = 0;
        _activeBulb = _heatBulbs[_activeBulbIndex];
        _activeBulbMinBorder = 0;
        _activeBulbMaxBorder = _activeBulbMinBorder + _bulbsStep;
    }

    public override void OnStartShooting(CancellationToken shootCT, float fireRate)
    {
        _inUse = true;
        ChargeAnimation(shootCT).Forget();
        abstractShootVFX.OnStartShooting(shootCT);
    }

    public override void Shoot()
    {
        abstractShootVFX.Shoot();
        CheckHitTask().Forget();
    }

    public override void StopShoot()
    {
        abstractShootVFX.StopShoot();
        _inUse = false;
        ShutdownAnimation().Forget();
    }

    async UniTaskVoid ChargeAnimation(CancellationToken shootCT)
    {
        while (!shootCT.IsCancellationRequested && !_onDestroyCTS.IsCancellationRequested)
        {
            _warmValue += Time.deltaTime;
            _warmValue = Mathf.Clamp01(_warmValue);
            UpdateHeatLines();

            if (_warmValue > _activeBulbMaxBorder)
            {
                _activeBulbIndex++;
                _activeBulb = _heatBulbs[_activeBulbIndex];

                _activeBulbMinBorder = _activeBulbMaxBorder;
                _activeBulbMaxBorder += _bulbsStep;
            }

            UpdateLastBulb();

            if (_warmValue == 1)
            {
                return;
            }
            await UniTask.Yield();
        }
    }

    void UpdateLastBulb()
    {
        float value = Mathf.InverseLerp(_activeBulbMinBorder, _activeBulbMaxBorder, _warmValue);
        _activeBulb.material.SetFloat(_lerpValuePropertyID, value);
    }



    async UniTaskVoid ShutdownAnimation()
    {
        while (!_inUse && !_onDestroyCTS.IsCancellationRequested)
        {
            _warmValue -= Time.deltaTime;
            _warmValue = Mathf.Clamp01(_warmValue);
            UpdateHeatLines();

            if (_warmValue < _activeBulbMinBorder)
            {
                _activeBulbIndex--;
                _activeBulb = _heatBulbs[_activeBulbIndex];

                _activeBulbMaxBorder -= _bulbsStep;
                _activeBulbMinBorder = _activeBulbMaxBorder - _bulbsStep;
            }

            UpdateLastBulb();

            if (_warmValue == 0) return;
            await UniTask.Yield();
        }
    }

    void UpdateHeatLines()
    {
        foreach (Renderer heatLine in _heatLines)
        {
            heatLine.material.SetFloat(_fillingValuePropertyID, _warmValue);
        }
    }

    async UniTaskVoid CheckHitTask()
    {
        while (_inUse && !_onDestroyCTS.IsCancellationRequested)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit firePointhitInfo, float.PositiveInfinity, _layerMaskForHit))
            {
                OnHit(firePointhitInfo.collider.gameObject, firePointhitInfo.point);
            }
            await UniTask.Yield();
        }
    }
}
