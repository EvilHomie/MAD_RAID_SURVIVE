using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DetachService : AbstractInRaidService
{
    CancellationTokenSource _inRaidCTS;

    List<Rigidbody> _detachedParts;


    protected override void OnStartRaid()
    {
        _detachedParts ??= new();
        _inRaidCTS = _inRaidCTS.Create();
        MoveDetachedPartsTask(_inRaidCTS.Token, destroyCancellationToken).Forget();
    }

    protected override void OnStopRaid()
    {
        _inRaidCTS.CancelAndDispose();
        _detachedParts.Clear();
    }

    public void DetachPart(IDetachable detachablePart)
    {
        detachablePart.GameObject.transform.parent = null;
        var Rb = detachablePart.GameObject.AddComponent<Rigidbody>();
        Rb.maxLinearVelocity = _config.MaxSpeedDetachedParts;

        DetachWithForce(detachablePart, Rb);
        _detachedParts.Add(Rb);
        _eventBus.OnVehiclePartDetached?.Invoke(detachablePart.GameObject.transform);
    }

    void DetachWithForce(IDetachable detachablePart, Rigidbody detachablePartRb)
    {
        Vector3 detachDirection = detachablePart.DetachDirectionGlobal switch
        {
            DetachDirection.ZDirection => Vector3.forward,
            DetachDirection.ZDirectionReversed => Vector3.back,
            DetachDirection.XDirection => Vector3.right / 2, //из расчета что вперед по движению частям сложнее отлететь
            DetachDirection.XDirectionReversed => Vector3.left,
            DetachDirection.YDirection => Vector3.up,
            DetachDirection.YDirectionReversed => Vector3.down,
            DetachDirection.ZYDirection => Vector3.forward + Vector3.up,
            DetachDirection.XYDirection => Vector3.right + Vector3.up,
            _ => Vector3.zero
        };

        detachablePartRb.AddForce(detachDirection * _config.DetachForce, ForceMode.Impulse);
    }


    async UniTaskVoid MoveDetachedPartsTask(CancellationToken stopRaidCT, CancellationToken dCT)
    {
        while (!dCT.IsCancellationRequested && !stopRaidCT.IsCancellationRequested)
        {
            for (int i = _detachedParts.Count - 1; i >= 0; i--)
            {
                if (_detachedParts[i] == null)
                {
                    _detachedParts.RemoveAt(i);
                    continue;
                }

                _detachedParts[i].AddForce(Vector3.left * _config.OnDetachedAccelerationSpeed, ForceMode.Acceleration);
            }
            await UniTask.Yield();
        }
    }


}
