using Cysharp.Threading.Tasks;

public class LaserBeam : WarmingWeapon
{
    private void Awake()
    {
        _onWarmed += OnWarmed;
    }

    protected override void OnStartShooting()
    {
        WarmUpTask(_shootingCTS.Token).Forget();
        GunPointsStartAnimation(_shootingCTS.Token).Forget();
    }

    protected override void OnStopShooting()
    {
        CoolingTask().Forget();
    }

    void OnWarmed()
    {
        ShootingTask(_shootingCTS.Token).Forget();
    }


}
