public class OtherAttachmentArmored : AbstractDetachableVehiclePart
{
    public override void Init(float hpMod, EnemyHpService enemyHpService, Enemy enemy)
    {
        _partType = VehiclePartType.OtherAttachmentArmored;
        base.Init(hpMod, enemyHpService, enemy);
    }
}
