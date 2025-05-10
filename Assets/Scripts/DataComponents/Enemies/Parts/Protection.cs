public class Protection : AbstractDetachableVehiclePart
{
    public override void Init(float hpMod, EnemyHpService enemyHpService, Enemy enemy)
    {
        _partType = VehiclePartType.Protection;
        base.Init(hpMod, enemyHpService, enemy);
    }
}
