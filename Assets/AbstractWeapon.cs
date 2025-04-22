using System.Threading;
using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour
{
    [SerializeField] AbstractGunPoint[] _gunPoints;
    [SerializeField] protected float _fireRate;
    [SerializeField] bool alternateShooting;

    protected CancellationTokenSource _shootingCTS;
    protected CancellationToken _onDestroyCTS;
    protected Config _config;
    int _nextGunPointForShootIndex = 0;

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
        _shootingCTS = _shootingCTS.Create();
    }
    public void StopShoot()
    {
        _shootingCTS.CancelAndDispose();
    }

    AbstractGunPoint NextGunPointShoot()
    {
        if (_gunPoints.Length > 1)
        {
            _nextGunPointForShootIndex++;
            if (_nextGunPointForShootIndex >= _gunPoints.Length) _nextGunPointForShootIndex = 0;
        }
        else _nextGunPointForShootIndex = 0;
        return _gunPoints[_nextGunPointForShootIndex];
    }

    protected void OnStartShoot(float animStartValue)
    {
        if (alternateShooting)
        {
            NextGunPointShoot().OnStartShoot(_shootingCTS.Token, _fireRate, animStartValue);
        }
        else
        {
            foreach (var point in _gunPoints)
            {
                point.OnStartShoot(_shootingCTS.Token, _fireRate, animStartValue);
            }
        }
    }
}
