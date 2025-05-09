using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class HitVisualService : AbstractInRaidService
{
    int _emissionHDRColorPropertyID;
    int _enableEmissionPropertyID;
    int _emissionValuePropertyID;
    CancellationTokenSource _inRaidCTS;
    List<IDamageable> _damagedParts;

    [Inject]
    public void Construct()
    {
        _emissionHDRColorPropertyID = Shader.PropertyToID("_EmissionHDRColor");
        _enableEmissionPropertyID = Shader.PropertyToID("_EnableEmission");
        _emissionValuePropertyID = Shader.PropertyToID("_EmissionValue");
        _damagedParts = new();
    }


    void OnDestroy()
    {
        _inRaidCTS?.CancelAndDispose();
    }

    protected override void OnStartRaid()
    {
        if (_config.ShowHitDuration > 0)
        {
            _inRaidCTS = _inRaidCTS.Create();
            EmissionsTask(_inRaidCTS.Token, destroyCancellationToken).Forget();
        }
    }

    protected override void OnStopRaid()
    {
        _inRaidCTS?.CancelAndDispose();
        _damagedParts.Clear();
    }


    public void OnPlayerDamageSomething(IDamageable damagedPart)
    {
        EnableHitEmission(damagedPart);
    }

    public void OnPartDestroyed(IDamageable damagedPart)
    {
        _damagedParts.Remove(damagedPart);
    }



    void EnableHitEmission(IDamageable damagedPart)
    {
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

        if (!damagedPart.EmissionInited)
        {
            foreach (var material in damagedPart.AssociatedMaterials)
            {
                damagedPart.EmissionInited = true;
                material.SetColor(_emissionHDRColorPropertyID, _config.CriticalHPColor);
                material.SetInt(_enableEmissionPropertyID, 1);
            }
        }
    }

    

    async UniTaskVoid EmissionsTask(CancellationToken stopRaidCT, CancellationToken dCT)
    {
        while (!dCT.IsCancellationRequested && !stopRaidCT.IsCancellationRequested)
        {
            for (int i = _damagedParts.Count - 1; i >= 0; i--)
            {
                if (_damagedParts[i] == null)
                {
                    _damagedParts.RemoveAt(i);
                    continue;
                }

                SetDamageEmission(_damagedParts[i]);
                _damagedParts[i].HitEmissionTimer -= Time.deltaTime;
                if (_damagedParts[i].HitEmissionTimer <= 0)
                {
                    foreach (var material in _damagedParts[i].AssociatedMaterials)
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
}
