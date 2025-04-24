using System.Threading;
using UnityEngine;

public abstract class AbstractGunPointFX : MonoBehaviour
{    
    protected CancellationToken _onDestroyCTS;
    protected Config _config;
    protected float _fireRate;
    protected bool _inUse;
    public virtual void Init(Config config, CancellationToken onDestroyCTS)
    {
        _config = config;
        _onDestroyCTS = onDestroyCTS;
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

}
