using UnityEngine;

public abstract class AbstractMoverPart : AbstractVehiclePart, IDetachable
{
    [SerializeField] protected bool _withSidewaysTurnAnimation;
    [SerializeField] DetachDirection _detachDirection;
    public abstract void MoveRotateAnimationTick(float tickValue, float adModValue = 0);
    public abstract void SidewaysTurnAnimationTick (float tickValue, float rotateSpeed);
    public DetachDirection DetachDirection { get => _detachDirection; }    
}
