using System.Threading;
using UnityEngine;

public abstract class AbstractGunPoint : MonoBehaviour
{
    [SerializeField] AbstractGunPointFX abstractShootVFX;
    protected CancellationToken _onDestroyCTS;
    protected Config _config;
    protected float _fireRate;
    protected bool _inUse;
    public virtual void Init(Config config, CancellationToken onDestroyCTS)
    {
        _config = config;
        _onDestroyCTS = onDestroyCTS;
        abstractShootVFX.Init(config, onDestroyCTS);
    }

    public virtual void OnStartShooting(CancellationToken shootCT, float fireRate)
    {        
        _fireRate = fireRate;
        _inUse = true;
        abstractShootVFX.OnStartShooting(shootCT, fireRate);
    }
    public virtual void OnStopShooting()
    {
        _inUse = false;
        abstractShootVFX.OnStopShooting();
    }
    public virtual void OnShoot()
    {
        abstractShootVFX.OnShoot();
    }
}
