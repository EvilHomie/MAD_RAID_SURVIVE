using Cysharp.Threading.Tasks;
using System.Threading;

public class MiniGun : WarmingWeapon
{
    public override void StartShoot()
    {
        base.StartShoot();
        WarmUpTask(_shootingCTS.Token).Forget();
        RdyForShootTask(_shootingCTS.Token).Forget();
    }

    public override void StopShoot()
    {
        base.StopShoot();
        CoolingTask().Forget();
    }

    async UniTaskVoid RdyForShootTask(CancellationToken shootCT)
    {
        while (!shootCT.IsCancellationRequested && !_onDestroyCTS.IsCancellationRequested)
        {
            if (_isWarmed)
            {
                OnReadyForShooting();
                return;
            }
            await UniTask.Yield();
        }
    }
}
