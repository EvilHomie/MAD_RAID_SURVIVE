public class Wheel : AbstractMoverPart
{
    public override void Init(float hpMod, EnemyHpService enemyHpService)
    {
        _partType = VehiclePartType.Wheel; 
        base.Init(hpMod, enemyHpService);
    }
}
