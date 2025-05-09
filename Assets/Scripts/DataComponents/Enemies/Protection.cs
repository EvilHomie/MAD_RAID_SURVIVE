using UnityEngine;

public class Protection : AbstractVehiclePart, IDetachable
{
    [SerializeField] DetachDirection _detachDirection;
    public DetachDirection DetachDirection { get => _detachDirection; }

    public override void Init(float hpMod, EnemyHpService enemyHpService, Enemy enemy)
    {
        _partType = VehiclePartType.Protection;
        base.Init(hpMod, enemyHpService, enemy);
    }
}
