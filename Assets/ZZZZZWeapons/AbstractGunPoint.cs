using System.Threading;
using UnityEngine;

public abstract class AbstractGunPoint : MonoBehaviour
{
    [SerializeField] protected AbstractGunPointFX abstractShootVFX;
    protected CancellationToken _onDestroyCTS;
    protected Config _config;
    public delegate void HitObjectCallBack(GameObject gameObject);
    HitObjectCallBack _hitcallback;
    public virtual void Init(Config config, CancellationToken onDestroyCTS, HitObjectCallBack callback)
    {
        _config = config;
        _onDestroyCTS = onDestroyCTS;
        abstractShootVFX.Init(config, onDestroyCTS);
        _hitcallback = callback;
        OnInit();
    }

    public abstract void OnInit();
    public abstract void OnStartShooting(CancellationToken shootCT, float fireRate = 0);
    public abstract void Shoot();
    public abstract void StopShoot();

    protected void OnHit(GameObject hitedObj, Vector3 pos)
    {
        _hitcallback(hitedObj);
        abstractShootVFX.OnHit(hitedObj, pos);
    }
}
