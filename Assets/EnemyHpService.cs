using Zenject;

public class EnemyHpService : AbstractInRaidService
{
    VehiclePartsHP _vehiclePartsHP;
    float _powerMod;

    [Inject]
    public void Construct(VehiclePartsHP vehiclePartsHP)
    {
        _vehiclePartsHP = vehiclePartsHP;
    }

    protected override void OnStartRaid()
    {
        _powerMod = 1;
        _eventBus.OnChangeEnemiesPower += (_power) => _powerMod = _power;
    }

    protected override void OnStopRaid()
    {
        _eventBus.OnChangeEnemiesPower -= (_power) => _powerMod = _power;
    }

    public float GetHPValueByType(VehiclePartType vehiclePartType)
    {
        return vehiclePartType switch
        {
            VehiclePartType.Wheel => _vehiclePartsHP.WheelHP * _powerMod,
            VehiclePartType.Caterpillar => _vehiclePartsHP.CaterpillarHP * _powerMod,
            VehiclePartType.ArmoredWheel => _vehiclePartsHP.ArmoredWheelHP * _powerMod,
            VehiclePartType.ExplosionPart => _vehiclePartsHP.ExplosionPartHP * _powerMod,
            VehiclePartType.Protection => _vehiclePartsHP.ProtectionHP * _powerMod,
            VehiclePartType.OtherAttachment => _vehiclePartsHP.OtherAttachmentHP * _powerMod,
            VehiclePartType.Weapon => _vehiclePartsHP.WeaponHP * _powerMod,
            _ => 0
        };
    }
}
