using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class EnvironmentService : MonoBehaviour
{
    Config _config;
    MeshRenderer _mainRoadRenderer;
    GameFlowService _gameFlowService;
    List<Transform> _spawnedBuilds;
    EventBus _eventBus;

    [Inject]
    public void Construct(Config config, MainRoad mainRoad, GameFlowService gameFlowService, EventBus eventBus)
    {
        _config = config;
        _mainRoadRenderer = mainRoad.GetComponent<MeshRenderer>();
        _gameFlowService = gameFlowService;
        _spawnedBuilds = new();
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

    private void OnSpawnEnvironmentObject(Transform t)
    {
        _spawnedBuilds.Add(t);
    }

    public void OnStopRaid()
    {
        _gameFlowService.CustomUpdate -= CustomUpdate;
        _spawnedBuilds.Clear();
        _eventBus.OnSpawnEnvironmentObject -= OnSpawnEnvironmentObject;
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
        for (int i = _spawnedBuilds.Count - 1; i >= 0; i--)
        {
            if (_spawnedBuilds[i].transform.position.x < _config.EnvironmentsAreaZone.XMin)
            {
                Destroy(_spawnedBuilds[i].gameObject);
                _spawnedBuilds.RemoveAt(i);
            }
            else
            {
                _spawnedBuilds[i].transform.Translate(_config.EnvironmentMoveSpeed * Time.deltaTime * Vector3.left, Space.World);
            }
        }
    }    
}
