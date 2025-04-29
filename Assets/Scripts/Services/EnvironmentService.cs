using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class EnvironmentService : MonoBehaviour
{
    Config _config;
    MeshRenderer _mainRoadRenderer;
    GameFlowService _gameFlowService;
    List<EnvironmentObject> _spawnedEnvObject;
    EventBus _eventBus;

    [Inject]
    public void Construct(Config config, MainRoad mainRoad, GameFlowService gameFlowService, EventBus eventBus)
    {
        _config = config;
        _mainRoadRenderer = mainRoad.GetComponent<MeshRenderer>();
        _gameFlowService = gameFlowService;
        _spawnedEnvObject = new();
        _eventBus = eventBus;

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

    public void OnStartRaid()
    {
        _gameFlowService.CustomUpdate += CustomUpdate;
        _eventBus.OnSpawnEnvironmentObject += OnSpawnEnvironmentObject;
    }    

    public void OnStopRaid()
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
