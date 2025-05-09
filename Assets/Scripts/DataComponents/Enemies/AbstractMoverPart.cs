using UnityEngine;

public abstract class AbstractMoverPart : AbstractVehiclePart, IDetachable
{
    [SerializeField] DetachDirection _detachDirection;
    public DetachDirection DetachDirection { get => _detachDirection; }


    [SerializeField] bool _withSidewaysTurnAnimation;
    Vector3 _sidewaysTargetRotation;
    public Vector3 SidewaysTargetRotation => _sidewaysTargetRotation;
    public bool WithSidewaysTurnAnimation => _withSidewaysTurnAnimation;
}
