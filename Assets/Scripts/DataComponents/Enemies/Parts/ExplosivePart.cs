public class ExplosivePart : AbstractDetachableVehiclePart
{
    public override void Init(float hpMod, EnemyHpService enemyHpService, Enemy enemy)
    {
        _partType = VehiclePartType.ExplosivePart;
        base.Init(hpMod, enemyHpService, enemy);
    }
}
