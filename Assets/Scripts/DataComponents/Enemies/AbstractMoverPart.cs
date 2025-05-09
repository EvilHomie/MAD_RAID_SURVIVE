using UnityEngine;

public abstract class AbstractMoverPart : AbstractVehiclePart, IDetachable
{
    [SerializeField] protected bool _withSidewaysTurnAnimation;
    [SerializeField] DetachDirection _detachDirection;
    public DetachDirection DetachDirection { get => _detachDirection; }
    public abstract void MoveForwardAnimationTick(float speed, float adModValue = 0);
    public abstract void UpdateSidewaysTurnAngle (float newAngle);

    public abstract void UpdateRotation(float rotateSpeed);
}
