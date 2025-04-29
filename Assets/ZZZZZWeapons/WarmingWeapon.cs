using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public abstract class WarmingWeapon : AbstractWeapon
{
    protected bool _isWarmed;
    protected float _warmingValue;
    bool _isCooled;

    protected Action _onWarmed;
    protected Action _onCooled;
    bool _inUse;

    void ChangeWarmingValue(float delta)
    {
        _warmingValue += delta;
        _warmingValue = Mathf.Clamp(_warmingValue, 0, _config.WarmingTime);
    }

    void CheckStatus()
    {
        if (_warmingValue == _config.WarmingTime && !_isWarmed)
        {
            _onWarmed?.Invoke();
            _isWarmed = true;
        }
        else if (_warmingValue == 0 && !_isCooled)
        {
            _onCooled?.Invoke();
            _isCooled = true;
        }
    }

    protected async UniTaskVoid WarmUpTask(CancellationToken shootCT)
    {
        _inUse = true;
        _isCooled = false;
        _isWarmed = false;

        while (!shootCT.IsCancellationRequested && !_onDestroyCTS.IsCancellationRequested)
        {
            ChangeWarmingValue(+Time.deltaTime);
            CheckStatus();
            if (_isWarmed)
            {                
                return;
            }
            await UniTask.Yield();
        }
    }

    protected async UniTaskVoid CoolingTask()
    {
        _inUse = false;
        _isCooled = false;
        _isWarmed = false;

        while (!_inUse && !_onDestroyCTS.IsCancellationRequested)
        {
            ChangeWarmingValue(-Time.deltaTime);
            CheckStatus();
            if (_isCooled) return;
            await UniTask.Yield();
        }
    }

    
}
