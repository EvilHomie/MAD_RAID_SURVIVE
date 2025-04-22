using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class WarmingWeapon : AbstractWeapon
{
    float _warmingValue;
    bool _isWarmed;
    bool _isCooled;
    bool _isWarming;
    bool _isCooling;


    public override void StartShoot()
    {
        base.StartShoot();
        ShootingTask(_shootingCTS.Token).Forget();
    }

    //void ChangeState(bool isShooting)
    //{
    //    _isWarming = isShooting;
    //    _isCooling = !isShooting;
    //    if (_isCooled && isShooting) _isCooled = false;
    //    if (_isWarmed && !isShooting) _isWarmed = false;
    //}

    void ChangeWarmingValue(float delta)
    {
        _warmingValue += delta;
        _warmingValue = Mathf.Clamp(_warmingValue, 0, _config.WarmingTime);
        CheckStatus();
    }

    void CheckStatus()
    {
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
        else
        {
            _isCooled = false;
            _isWarmed = false;
        }
    }

    async UniTaskVoid ShootingTask(CancellationToken shootCT)
    {
        _isWarming = true;
        _isCooling = false;
        OnStartShoot(_warmingValue);

        while (!shootCT.IsCancellationRequested && !_onDestroyCTS.IsCancellationRequested)
        {
            if (_isWarmed)
            {
                await UniTask.Yield();
                continue;
            }
            ChangeWarmingValue(+Time.deltaTime);
            await UniTask.Yield();
        }

        _isWarming = false;
        _isCooling = true;

        while (_isCooling && !_onDestroyCTS.IsCancellationRequested)
        {
            ChangeWarmingValue(-Time.deltaTime);
            await UniTask.Yield();
        }
    }
}
