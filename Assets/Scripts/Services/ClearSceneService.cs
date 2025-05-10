using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class ClearSceneService : AbstractInRaidService
{
    List<Transform> _trackingEnvirTransforms;
    List<Transform> _trackingVehicleParts;
    CancellationTokenSource _ctsOnStopRaid;


    [Inject]
    public void Construct()
    {
        _trackingEnvirTransforms = new();
        _trackingVehicleParts = new();
    }
    private void OnEnable()
    {
        _eventBus.OnStartRaid += OnStartRaid;
        _eventBus.OnStopRaid += OnStopRaid;
    }

    private void OnDisable()
    {
        _eventBus.OnStartRaid -= OnStartRaid;
        _eventBus.OnStopRaid -= OnStopRaid;
    }
    protected override void OnStartRaid()
    {
        _eventBus.OnEnemyDie += (enemy) => AddVehiclePartForTrack(enemy.Rigidbody.transform);
        _eventBus.OnVehiclePartDetached += AddVehiclePartForTrack;
        _eventBus.OnSpawnEnvironmentObject += (MB) => AddEnviromentForTrack(MB.transform);

        _ctsOnStopRaid = _ctsOnStopRaid.Create();
        CheckTransformsForDestroy(_ctsOnStopRaid.Token).Forget();
    }

    protected override void OnStopRaid()
    {
        _eventBus.OnEnemyDie -= (enemy) => AddVehiclePartForTrack(enemy.Rigidbody.transform);
        _eventBus.OnVehiclePartDetached -= AddVehiclePartForTrack;
        _eventBus.OnSpawnEnvironmentObject -= (MB) => AddEnviromentForTrack(MB.transform);

        _ctsOnStopRaid.CancelAndDispose();
        foreach (var transform in _trackingEnvirTransforms)
        {
            Destroy(transform.gameObject);
        }
        _trackingEnvirTransforms.Clear();
        foreach (var transform in _trackingVehicleParts)
        {
            Destroy(transform.gameObject);
        }
        _trackingVehicleParts.Clear();
    }

    void AddEnviromentForTrack(Transform transform)
    {
        _trackingEnvirTransforms.Add(transform);
    }
    void AddVehiclePartForTrack(Transform transform)
    {
        _trackingVehicleParts.Add(transform);
    }

    async UniTaskVoid CheckTransformsForDestroy(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_config.CheckObjectForDestroyRepeatDelay), ignoreTimeScale: false, cancellationToken: ct);
            if (_trackingEnvirTransforms.Count != 0)
            {
                for (int i = _trackingEnvirTransforms.Count - 1; i >= 0; i--)
                {
                    if (_trackingEnvirTransforms[i].position.x < _config.EnvironmentsAreaZone.XMin)
                    {
                        Destroy(_trackingEnvirTransforms[i].root.gameObject);
                        _trackingEnvirTransforms.RemoveAt(i);
                    }
                }
            }
            if (_trackingVehicleParts.Count != 0)
            {
                for (int i = _trackingVehicleParts.Count - 1; i >= 0; i--)
                {
                    Debug.Log(_trackingVehicleParts[i].position.x);
                    if (_trackingVehicleParts[i].position.x < _config.DestroyVehiclePartsPos)
                    {
                        Destroy(_trackingVehicleParts[i].root.gameObject);
                        _trackingVehicleParts.RemoveAt(i);
                    }
                }
            }


        }
    }
}
