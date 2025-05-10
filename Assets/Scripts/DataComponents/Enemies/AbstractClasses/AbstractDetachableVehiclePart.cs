using UnityEngine;

public class AbstractDetachableVehiclePart : AbstractVehiclePart, IDetachable
{
    [SerializeField] DetachDirection _detachDirectionGlobal;
    public DetachDirection DetachDirectionGlobal { get => _detachDirectionGlobal; }
}
