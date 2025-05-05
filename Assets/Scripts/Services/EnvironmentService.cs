using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class EnvironmentService : AbstractInRaidService
{
    MeshRenderer _mainRoadRenderer;
    List<EnvironmentObject> _spawnedEnvObject;

    [Inject]
    public void Construct(MainRoad mainRoad)
    {
        _mainRoadRenderer = mainRoad.GetComponent<MeshRenderer>();
        _spawnedEnvObject = new();

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
        _gameFlowService.CustomUpdate += CustomUpdate;
        _eventBus.OnSpawnEnvironmentObject += OnSpawnEnvironmentObject;
    }

    protected override void OnStopRaid()
    {
        _gameFlowService.CustomUpdate -= CustomUpdate;
        _eventBus.OnSpawnEnvironmentObject -= OnSpawnEnvironmentObject;
        _spawnedEnvObject.Clear();
    }

    private void OnSpawnEnvironmentObject(EnvironmentObject environmentObject)
    {
        _spawnedEnvObject.Add(environmentObject);
    }

    private void CustomUpdate()
    {
        SimulateMainRoadMove();
        MoveEnvironmentObject();
    }

    void SimulateMainRoadMove()
    {
        _mainRoadRenderer.material.mainTextureOffset += _config.GroundMoveSpeed * Time.deltaTime * Vector2.left;
    }

    void MoveEnvironmentObject()
    {
        for (int i = _spawnedEnvObject.Count - 1; i >= 0; i--)
        {
            if (_spawnedEnvObject[i] == null)
            {
                _spawnedEnvObject.RemoveAt(i);
                continue;
            }
            _spawnedEnvObject[i].transform.Translate(_config.EnvironmentMoveSpeed * Time.deltaTime * Vector3.left, Space.World);
        }
    }    
}
