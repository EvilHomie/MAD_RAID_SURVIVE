using UnityEngine;

public class Wheel : AbstractMoverPart
{
    Vector3 _rotationVector = Vector3.right;

    public override void Init(float hpMod, EnemyHpService enemyHpService)
    {
        _partType = VehiclePartType.Wheel;
        base.Init(hpMod, enemyHpService);
    }

    public override void MoveRotateAnimationTick(float tickValue, float adModValue = 0)
    {
        transform.Rotate(Vector3.right, tickValue, Space.Self);
    }

    public override void SidewaysTurnAnimationTick(float rotateValue, float rotateSpeed)
    {
        if (!_withSidewaysTurnAnimation) return;
        _rotationVector.y = rotateValue;
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(_rotationVector), rotateSpeed);
    }
}
