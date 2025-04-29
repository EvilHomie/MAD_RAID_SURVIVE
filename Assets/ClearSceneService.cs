using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class ClearSceneService : MonoBehaviour
{
    EventBus _eventBus;
    Config _config;

    List<Transform> _trackingEnvirTransforms;
    List<Transform> _trackingVehicleParts;
    CancellationTokenSource _ctsOnStopRaid;


    [Inject]
    public void Construct(Config config, EventBus eventBus)
    {
        _config = config;
        _eventBus = eventBus;
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
    private void OnStartRaid()
    {
        _eventBus.OnSpawnEnemy += AddVehiclePartFOrTrack;
        _eventBus.OnVehiclePartDie += AddVehiclePartFOrTrack;
        _eventBus.OnSpawnEnvironmentObject += AddTransformToCollection;

        _ctsOnStopRaid = _ctsOnStopRaid.Create();
        CheckTransformsForDestroy(_ctsOnStopRaid.Token).Forget();
    }

    void AddTransformToCollection(MonoBehaviour monoBehaviour)
    {
        _trackingEnvirTransforms.Add(monoBehaviour.transform);
    }
    void AddVehiclePartFOrTrack(MonoBehaviour monoBehaviour)
    {
        _trackingVehicleParts.Add(monoBehaviour.transform);
    }

    private void OnStopRaid()
    {
        _eventBus.OnSpawnEnemy -= AddTransformToCollection;
        _eventBus.OnVehiclePartDie -= AddTransformToCollection;
        _eventBus.OnSpawnEnvironmentObject -= AddTransformToCollection;

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
                        Destroy(_trackingEnvirTransforms[i].gameObject);
                        _trackingEnvirTransforms.RemoveAt(i);
                    }
                }
            }
            if (_trackingVehicleParts.Count != 0)
            {
                for (int i = _trackingVehicleParts.Count - 1; i >= 0; i--)
                {
                    if (_trackingVehicleParts[i].position.x < _config.DestroyVehiclePartsPos)
                    {
                        Destroy(_trackingVehicleParts[i].gameObject);
                        _trackingVehicleParts.RemoveAt(i);
                    }
                }
            }


        }
    }
}
