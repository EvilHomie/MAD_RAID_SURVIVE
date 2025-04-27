using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public abstract class AbstractGunPointFX : MonoBehaviour
{
    [SerializeField] protected Light _light;
    protected CancellationToken _onDestroyCTS;
    protected CancellationToken _shootCT;
    protected Config _config;
    protected float _fireRate;
    protected bool _inUse;
    public virtual void Init(Config config, CancellationToken onDestroyCTS)
    {
        _config = config;
        _onDestroyCTS = onDestroyCTS;
        _light.enabled = false;
    }

    public virtual void OnStartShooting(CancellationToken shootCT, float fireRate)
    {
        _fireRate = fireRate;
        _inUse = true;
    }
    public virtual void OnStopShooting()
    {
        _inUse = false;
    }
    public virtual void OnShoot()
    {

    }

    protected async UniTaskVoid LightFlickerTask(float duration)
    {
        if (_light.enabled) return;
        _light.enabled = true;
        await UniTask.Delay(TimeSpan.FromSeconds(duration), ignoreTimeScale: false, cancellationToken: _onDestroyCTS);
        _light.enabled = false;
    }

}
