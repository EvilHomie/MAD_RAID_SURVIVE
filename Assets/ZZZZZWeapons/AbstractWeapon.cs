using Cysharp.Threading.Tasks;
using System;
using System.Drawing;
using System.Threading;
using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour
{
    [SerializeField] protected AbstractGunPoint[] _gunPoints;
    [SerializeField] protected float _fireRate;
    [SerializeField] protected bool alternateShooting;

    protected CancellationTokenSource _shootingCTS;
    protected CancellationToken _onDestroyCTS;
    protected Config _config;

    protected int _nextGunPointForShootIndex = 0;
    float _nextTimeTofire = 0;
    protected bool _inUse;


    public void Init(Config config, CancellationToken onDestroyCTS)
    {
        _config = config;
        _onDestroyCTS = onDestroyCTS;
        foreach (var point in _gunPoints) point.Init(config, onDestroyCTS);
    }


    public void StartShoot()
    {
        _inUse = true;
        _shootingCTS = _shootingCTS.Create();
        OnStartShooting();
    }
    public void StopShoot()
    {
        _inUse = false;
        _shootingCTS.CancelAndDispose();
        foreach (var point in _gunPoints)
        {
            point.OnStopShooting();
        }
        OnStopShooting();
    }

    protected abstract void OnStartShooting();
    protected abstract void OnStopShooting();

    public void Aim(Vector3 targetPos)
    {
        float singleStep = Time.deltaTime;
        Vector3 targetDirection = targetPos - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep * 5, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);




        //transform.LookAt(targetPos);
        foreach (var point in _gunPoints)
        {
            Vector3 dir = targetPos - point.transform.position;
            Vector3 newDir = Vector3.RotateTowards(point.transform.forward, dir, singleStep, 0.0f);
            point.transform.rotation = Quaternion.LookRotation(newDir);
            //point.transform.LookAt(targetPos);
        }



    }

    void EmitShoot()
    {
        if (alternateShooting) NextGunPointForShoot().OnShoot();
        else foreach (var point in _gunPoints) point.OnShoot();
    }

    protected async UniTaskVoid ShootingTaskWithFireRate(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested && !_onDestroyCTS.IsCancellationRequested)
        {
            if (Time.time >= _nextTimeTofire)
            {
                _nextTimeTofire = Time.time + 1f / _fireRate;
                EmitShoot();
            }
            await UniTask.Yield();
        }
    }

    protected AbstractGunPoint NextGunPointForShoot()
    {
        if (_gunPoints.Length > 1)
        {
            _nextGunPointForShootIndex++;
            if (_nextGunPointForShootIndex >= _gunPoints.Length) _nextGunPointForShootIndex = 0;
        }
        else _nextGunPointForShootIndex = 0;
        return _gunPoints[_nextGunPointForShootIndex];
    }

    protected async UniTask GunPointsStartAnimation(CancellationToken shootCT)
    {
        int index = _nextGunPointForShootIndex;
        if (alternateShooting)
        {
            for (int i = 0; i < _gunPoints.Length; i++)
            {
                index++;
                if (index >= _gunPoints.Length) index = 0;
                _gunPoints[index].OnStartShooting(shootCT, _fireRate);
                await UniTask.Delay(TimeSpan.FromSeconds(1 / _fireRate), ignoreTimeScale: false, cancellationToken: shootCT);
            }
        }
        else foreach (var point in _gunPoints) point.OnStartShooting(shootCT, _fireRate);
    }
}
