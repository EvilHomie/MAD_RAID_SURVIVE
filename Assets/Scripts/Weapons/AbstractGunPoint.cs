using System.Threading;
using UnityEngine;

public abstract class AbstractGunPoint : MonoBehaviour
{    
    protected CancellationToken _onDestroyCTS;
    protected Config _config;
    public virtual void Init(Config config, CancellationToken onDestroyCTS)
    {
        _config = config;
        _onDestroyCTS = onDestroyCTS;        
    }

    public abstract void OnStartShoot(CancellationToken shootCT, float fireRate,  float startAnimValue);
    public void OnShoot()
    {

    }
}
