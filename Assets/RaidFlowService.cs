using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class RaidFlowService : AbstractInRaidService
{
    CancellationTokenSource _ctsOnStopRaid;
    float _enemyPowerMod;
    int _enemyTir;

    protected override void OnStartRaid()
    {
        _enemyPowerMod = 1;
        _enemyTir = 1;
        _ctsOnStopRaid = _ctsOnStopRaid.Create();
        IncreaseEnemyPowerTask(_ctsOnStopRaid.Token).Forget();
    }

    protected override void OnStopRaid()
    {
        _ctsOnStopRaid.CancelAndDispose();
    }

    async UniTaskVoid IncreaseEnemyPowerTask(CancellationToken ct)
    {
        float timer = 0;
        while (!ct.IsCancellationRequested && !destroyCancellationToken.IsCancellationRequested)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_config.IncreaseEnemyPowerTickRate), ignoreTimeScale: false, cancellationToken: ct);
            timer += _config.IncreaseEnemyPowerTickRate;

            if (timer < _config.IncreaseEnemyTirDelay)
            {
                IncreaseEnemyPower();
            }
            else
            {
                timer = 0;
                ResetPower();
                IncreaseEnemyTir();
            }
        }
    }

    void IncreaseEnemyPower()
    {
        _enemyPowerMod += _config.IncreaseEnemyPowerTickRate * _config.IncreaseEnemyPowerValueByTime;
        _eventBus.OnChangeEnemiesPower?.Invoke(_enemyPowerMod);
    }

    void IncreaseEnemyTir()
    {
        _enemyTir++;
        _eventBus.OnChangeEnemiesTir?.Invoke(_enemyTir);
    }

    void ResetPower()
    {
        _enemyPowerMod = 1;
        _eventBus.OnChangeEnemiesPower?.Invoke(_enemyPowerMod);
    }
}
