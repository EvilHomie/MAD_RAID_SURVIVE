public abstract class InstantShootWeapon : AbstractWeapon
{
    public override void StartShoot()
    {
        base.StartShoot();
        OnReadyForShooting();
    }
}
