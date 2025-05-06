using UnityEngine;

public abstract class AbstractMoverPart : AbstractVehiclePart
{
    [SerializeField] protected bool _withSidewaysTurnAnimation;
    public abstract void MoveRotateAnimationTick(float tickValue, float adModValue = 0);

    public abstract void SidewaysTurnAnimationTick (float tickValue, float rotateSpeed);
}
