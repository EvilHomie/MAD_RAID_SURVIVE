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
    public void Init(Config config, CancellationToken onDestroyCTS)
    {
        _config = config;
        _onDestroyCTS = onDestroyCTS;
        _light.enabled = false;
        OnInit();
    }

    protected async UniTaskVoid LightFlickerTask(float duration)
    {
        if (_light.enabled) return;
        _light.enabled = true;
        await UniTask.Delay(TimeSpan.FromSeconds(duration), ignoreTimeScale: false, cancellationToken: _onDestroyCTS);
        _light.enabled = false;
    }

    public abstract void OnInit();
    public abstract void OnStartShooting(CancellationToken shootCT, float fireRate = 0);
    public abstract void Shoot();
    public abstract void StopShoot();
    public abstract void OnHit(GameObject hitedObj, Vector3 pos);

}
