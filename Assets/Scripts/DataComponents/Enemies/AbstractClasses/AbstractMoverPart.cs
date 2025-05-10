using UnityEngine;

public abstract class AbstractMoverPart : AbstractDetachableVehiclePart
{
    [SerializeField] bool _withSidewaysTurnAnimation;
    Vector3 _sidewaysTargetRotation;
    public Vector3 SidewaysTargetRotation => _sidewaysTargetRotation;
    public bool WithSidewaysTurnAnimation => _withSidewaysTurnAnimation;
}
