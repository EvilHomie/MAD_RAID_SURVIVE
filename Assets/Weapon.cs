using System;
using System.Threading;
using UnityEngine;
using Zenject;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] GunPoint[] _gunPoints;
    [SerializeField] protected float _fireRate;
    protected int GunCount => _gunPoints.Length;    
    protected GunpointAnimationService _gunpointAnimationService;
    protected CancellationTokenSource _shootCTS;

    protected Action _shootEvent;
    private int _nextGunPointForShootIndex = 0;
    [Inject]
    public void Construct(GunpointAnimationService gunpointAnimationService)
    {
        _gunpointAnimationService = gunpointAnimationService;
    }
    public abstract void OnStartShoot();
    public abstract void OnStopShoot();

    public GunPoint GunPoinForNextShoot()
    {
        if (_gunPoints.Length > 1)
        {
            _nextGunPointForShootIndex++;
            if (_nextGunPointForShootIndex >= _gunPoints.Length) _nextGunPointForShootIndex = 0;
        }
        else _nextGunPointForShootIndex = 0;
        return _gunPoints[_nextGunPointForShootIndex];
    }
}
