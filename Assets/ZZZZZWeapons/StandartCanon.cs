public class StandartCanon : AbstractWeapon
{
    protected override void OnStartShooting()
    {
        foreach (var point in _gunPoints)
        {
            point.OnStartShooting(_shootingCTS.Token, _fireRate);
        }
        ShootingTask(_shootingCTS.Token).Forget();
    }

    protected override void OnStopShooting()
    {
        return;
    }
}
