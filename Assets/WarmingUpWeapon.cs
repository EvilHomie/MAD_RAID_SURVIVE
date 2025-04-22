using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

public class WarmingUpWeapon : Weapon
{
    float _warmingValue;
    bool _isWarmed;
    bool _isCooled;
    bool _isWarming;
    bool _isCooling;
    protected Config _config;

    [Inject]
    public void Construct(Config config)
    {
        _config = config;
    }
    void ChangeShootingState(bool isShooting)
    {
        _isWarming = isShooting;
        _isCooling = !isShooting;
        if (_isCooled && isShooting) _isCooled = false;
        if (_isWarmed && !isShooting) _isWarmed = false;
    }

    void ChangeWarmingValue(float delta)
    {
        _warmingValue += delta;
        _warmingValue = Mathf.Clamp(_warmingValue, 0, _config.WarmingTime);
        if (_warmingValue == _config.WarmingTime && !_isWarmed)
        {
            _isWarmed = true;
            _isWarming = false;
        }
        else if (_warmingValue == 0 && !_isCooled)
        {
            _isCooled = true;
            _isCooling = false;
        }
    }

    public override void OnStartShoot()
    {
        ChangeShootingState(true);
        _shootCTS = _shootCTS.Create();
        MachineGunWeaponShootingTask(_shootCTS.Token).Forget();
    }

    public override void OnStopShoot()
    {
        ChangeShootingState(false);
        _shootCTS.CancelAndDispose();
    }


    async UniTaskVoid MachineGunWeaponShootingTask(CancellationToken shootCT)
    {
        CancellationTokenSource coolingCTS = new();

        for (int i = 0; i < GunCount; i++)
        {
            Vector3 directionMod = (i + 1) % 2 == 0 ? Vector3.back : Vector3.forward; 
            _gunpointAnimationService.GunPointWarmingUpRotateAnimation(shootCT, _fireRate, _warmingValue, GunPoinForNextShoot(), directionMod).Forget();
            
        }
        while (!shootCT.IsCancellationRequested)
        {
            if (_isWarmed)
            {
                await UniTask.Yield();
                continue;
            }
            ChangeWarmingValue(+Time.deltaTime);
            await UniTask.Yield();
        }

        for (int i = 0; i < GunCount; i++)
        {
            Vector3 directionMod = (i + 1) % 2 == 0 ? Vector3.back : Vector3.forward;
            _gunpointAnimationService.GunPointCoolingRotateAnimation(coolingCTS.Token, _fireRate, _warmingValue, GunPoinForNextShoot(), directionMod).Forget();
        }
        while (!coolingCTS.IsCancellationRequested)
        {
            if (_isCooled || _isWarming)
            {
                coolingCTS.CancelAndDispose();
                return;
            }
            ChangeWarmingValue(-Time.deltaTime);
            await UniTask.Yield();
        }

    }
}
