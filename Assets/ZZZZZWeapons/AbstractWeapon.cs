using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour
{
    [SerializeField] protected AbstractGunPoint[] _gunPoints;
    [SerializeField] protected float _fireRate;
    [SerializeField] protected bool alternateShooting;

    protected CancellationTokenSource _shootingCTS;
    protected CancellationToken _onDestroyCTS;
    protected Config _config;
    protected bool _inUse;

    int _nextGunPointForShootIndex = 0;
    float _nextTimeTofire = 0;

    public void Init(Config config, CancellationToken onDestroyCTS)
    {
        _config = config;
        _onDestroyCTS = onDestroyCTS;
        foreach (var point in _gunPoints)
        {
            point.Init(config, onDestroyCTS);
        }
    }

    public virtual void StartShoot()
    {
        _inUse = true;
        _shootingCTS = _shootingCTS.Create();
        OnStartShooting().Forget();
    }
    public virtual void StopShoot()
    {
        _inUse = false;
        _shootingCTS.CancelAndDispose();
        OnStopShooting();
    }

    async UniTaskVoid OnStartShooting()
    {        
        for (int i = 0; i < _gunPoints.Length; i++)
        {
            NextGunPointShoot().OnStartShooting(_shootingCTS.Token, _fireRate);
            if (alternateShooting)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1 / _fireRate), ignoreTimeScale: false, cancellationToken: _shootingCTS.Token);
            }
        }
    }

    void OnStopShooting()
    {
        foreach (var point in _gunPoints)
        {
            point.OnStopShooting();
        }

        //_nextGunPointForShootIndex++;
        //for (int i = 0; i < _gunPoints.Length; i++)
        //{
        //    NextGunPointShoot().OnStopShooting();
        //    if (alternateShooting)
        //    {
        //        await UniTask.Delay(TimeSpan.FromSeconds(1 / _fireRate), ignoreTimeScale: false);
        //    }
        //}
    }

    protected void OnReadyForShooting()
    {
        EmitingShootTask(_shootingCTS.Token).Forget();
    }

    void EmitShoot()
    {
        if (alternateShooting)
        {
            NextGunPointShoot().EmitShoot();
        }
        else
        {
            foreach (var point in _gunPoints)
            {
                point.EmitShoot();
            }
        }
    }

    async UniTaskVoid EmitingShootTask(CancellationToken shootCT)
    {
        while (!shootCT.IsCancellationRequested)
        {
            if (Time.time >= _nextTimeTofire)
            {
                _nextTimeTofire = Time.time + 1f / _fireRate;
                EmitShoot();
            }
            await UniTask.Yield();
        }
    }

    AbstractGunPoint NextGunPointShoot()
    {
        if (_gunPoints.Length > 1)
        {
            _nextGunPointForShootIndex++;
            if (_nextGunPointForShootIndex >= _gunPoints.Length) _nextGunPointForShootIndex = 0;
        }
        else _nextGunPointForShootIndex = 0;

        Debug.Log(_gunPoints[_nextGunPointForShootIndex].name);
        return _gunPoints[_nextGunPointForShootIndex];
    }
}
