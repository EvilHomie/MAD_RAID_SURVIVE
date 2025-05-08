using UnityEngine;

public class VehicleBody : AbstractVehiclePart
{
    Vector3 _rotationVector = Vector3.up;
    float _rootRotationValue;

    public override void Init(float hpMod, EnemyHpService enemyHpService)
    {
        _rootRotationValue = transform.root.rotation.eulerAngles.y;
        _partType = VehiclePartType.Wheel;
        base.Init(hpMod, enemyHpService);
    }

    public void SidewaysTurnAnimationTick(float rotateValue, float rotateSpeed)
    {
        _rotationVector.y = rotateValue + _rootRotationValue;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(_rotationVector), rotateSpeed);
    }
}
