using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour
{
    [SerializeField] protected AbstractGunPoint[] _gunPoints;
    [SerializeField] protected bool _alternateShooting;
    [SerializeField] protected float _damage;
    protected CancellationTokenSource _shootingCTS;
    protected CancellationToken _onDestroyCTS;
    protected Config _config;
    protected HitService _hitService;

    protected int _lastGunPointIndex = 0;

    public delegate void HitObjectCallBack(GameObject gameObject, Vector3 hitPos, float damage);
   protected HitObjectCallBack _hitcallback;


    public void Init(Config config, CancellationToken onDestroyCTS, HitObjectCallBack callback)
    {
        _config = config;
        _onDestroyCTS = onDestroyCTS;
        _hitcallback = callback;
        foreach (var point in _gunPoints)
        {
            point.Init(config, onDestroyCTS, OnHitGameObject);

        }
    }
    public void StartShooting()
    {
        _shootingCTS = _shootingCTS.Create();
        OnStartShooting();
    }
    public void StopShooting()
    {
        _shootingCTS.CancelAndDispose();
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

        foreach (var point in _gunPoints)
        {
            Vector3 dir = targetPos - point.transform.position;
            Vector3 newDir = Vector3.RotateTowards(point.transform.forward, dir, singleStep, 0.0f);
            point.transform.rotation = Quaternion.LookRotation(newDir);
        }
    }
    protected AbstractGunPoint NextGunPoint()
    {
        if (_gunPoints.Length > 1)
        {
            _lastGunPointIndex++;
            if (_lastGunPointIndex >= _gunPoints.Length) _lastGunPointIndex = 0;
        }
        else _lastGunPointIndex = 0;
        return _gunPoints[_lastGunPointIndex];
    }

    protected virtual async UniTaskVoid ShootingTask(CancellationToken cancellationToken)
    {
        await UniTask.Yield(); // чтобы убрать предупреждение. “. . async об€зан быть с телом нет возможности применить abstract 
    }

    protected abstract void OnHitGameObject(GameObject hitedObject, Vector3 hitPos);
}
