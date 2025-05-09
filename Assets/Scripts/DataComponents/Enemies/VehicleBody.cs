public class VehicleBody : AbstractVehiclePart
{
    public override void Init(float hpMod, EnemyHpService enemyHpService, Enemy enemy)
    {
        _partType = VehiclePartType.Body;
        base.Init(hpMod, enemyHpService, enemy);
    }
}
