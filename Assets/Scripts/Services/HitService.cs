using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class HitService : AbstractInRaidService
{
    int _emissionHDRColorPropertyID;
    int _enableEmissionPropertyID;
    int _emissionValuePropertyID;
    protected CancellationTokenSource _inRaidCTS;
    List<IDamageable> _damagedParts;

    [Inject]
    public void Construct()
    {
        _emissionHDRColorPropertyID = Shader.PropertyToID("_EmissionHDRColor");
        _enableEmissionPropertyID = Shader.PropertyToID("_EnableEmission");
        _emissionValuePropertyID = Shader.PropertyToID("_EmissionValue");
        _damagedParts = new();
    }
    protected override void OnStartRaid()
    {
        _eventBus.OnPlayerHitsSomething += OnPlayerHitsSomething;
        if (_config.ShowHitDuration > 0)
        {
            _inRaidCTS = _inRaidCTS.Create();
            EmissionsTask(_inRaidCTS.Token).Forget();
        }
    }
    protected override void OnStopRaid()
    {
        _eventBus.OnPlayerHitsSomething -= OnPlayerHitsSomething;
        _inRaidCTS?.CancelAndDispose();
        _damagedParts.Clear();
    }

    private void OnPlayerHitsSomething(GameObject hitedObj, Vector3 hitPos, float damage)
    {
        if (hitedObj.TryGetComponent<IDamageable>(out var damagedPart))
        {
            if (damagedPart.CurrentHPValue <= 0) return;

            damagedPart.OnDamaged(damage);

            if (damagedPart.CurrentHPValue > 0)
            {
                EnableHitEmission(damagedPart);
            }
            else
            {
                _damagedParts.Remove(damagedPart);
                if (hitedObj.TryGetComponent<IDetachable>(out var detachedPart))
                {
                    Detach(detachedPart);
                }
            }
        }
    }

    void EnableHitEmission(IDamageable damagedPart)
    {      
        if (!damagedPart.ColorInited)
        {
            foreach (var material in damagedPart.AssociatedMaterials)
            {
                damagedPart.ColorInited = true;
                material.SetColor(_emissionHDRColorPropertyID, _config.CriticalHPColor);
                material.SetInt(_enableEmissionPropertyID, 1);
            }
        }

        if (_config.ShowHitDuration > 0)
        {
            if (damagedPart.HitEmissionTimer <= 0)
            {
                foreach (var material in damagedPart.AssociatedMaterials) material.SetInt(_enableEmissionPropertyID, 1);
            }

            damagedPart.HitEmissionTimer = _config.ShowHitDuration;
            if (!_damagedParts.Contains(damagedPart)) _damagedParts.Add(damagedPart);
        }
        else
        {
            SetDamageEmission(damagedPart);
        }

        //if (!_damagedParts.Contains(damagedPart))
        //{
        //    foreach (var material in damagedPart.AssociatedMaterials)
        //    {
        //        material.SetColor(_emissionHDRColorPropertyID, _config.CriticalHPColor);
        //        material.SetInt(_enableEmisionPropertyID, 1);
        //    }

        //    if (_config.ShowHitDuration > 0)
        //    {
        //        damagedPart.HitEmissionTimer = _config.ShowHitDuration;
        //        _damagedParts.Add(damagedPart);
        //    }
        //}
        //else
        //{
        //    SetDamageEmission(damagedPart);
        //}
    }

    void Detach(IDetachable detachedPart)
    {
        Debug.Log("DETACHED!!!!");
    }

    async UniTaskVoid EmissionsTask(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested && !destroyCancellationToken.IsCancellationRequested)
        {
            foreach (var part in _damagedParts)
            {
                SetDamageEmission(part);
                part.HitEmissionTimer -= Time.deltaTime;
                if (part.HitEmissionTimer <= 0)
                {
                    foreach (var material in part.AssociatedMaterials)
                    {
                        material.SetInt(_enableEmissionPropertyID, 0);
                    }
                }
            }
            await UniTask.Yield();
        }
    }



    void SetDamageEmission(IDamageable damagedPart)
    {
        foreach (var material in damagedPart.AssociatedMaterials)
        {
            float damageInterpolation = Mathf.InverseLerp(damagedPart.MaxHPValue, 0, damagedPart.CurrentHPValue);
            material.SetFloat(_emissionValuePropertyID, damageInterpolation);
        }
    }





    /*
     * public void OnDamaged(float hitValue, float showDuration)
    {
        _currentHPValue -= hitValue;

        _lastHitTime = Time.time;
        if (_emissionTask.Status.IsCompleted())
        {
            _emissionTask = OnDamagedEmissionTask(showDuration);
        }
    }


    async UniTask OnDamagedEmissionTask(float duration)
    {
        float timer = Time.time;
        if (duration == 0)
        {
            SetDamageEmission();
        }
        else
        {
            while (timer <= _lastHitTime + duration && !destroyCancellationToken.IsCancellationRequested)
            {
                timer += Time.deltaTime;
                SetDamageEmission();
                await UniTask.Yield();
            }
            foreach (var renderer in _associatedRenderers)
            {
                renderer.material.SetColor(_emissionValuePropertyID, Color.black);

            }
        }
    }

    void SetDamageEmission()
    {
        foreach (var renderer in _associatedRenderers)
        {
            float damageInterpolation = Mathf.InverseLerp(_maxHpValue, 0, _currentHPValue);
            Color color = _criticalHPColor * _config.CriticalHPCurve.Evaluate(damageInterpolation);
            renderer.material.SetColor(_emissionValuePropertyID, color);
        }
    }*/

}
