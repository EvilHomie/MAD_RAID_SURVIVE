using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;


public class EnvironmentService : MonoBehaviour
{
    Dictionary<BuildSize, Transform[]> _buildingPrefabsBysize;
    Config _config;
    MeshRenderer _mainRoadRenderer;
    GameFlowService _gameFlowService;
    List<Transform> _spawnedBuilds;
    CancellationTokenSource ctsOnStopRaid;

    int _buildSizeCount;


    [Inject]
    public void Construct(Config config, LargeBuildingCollection largeBuildings, MediumBuildingCollection mediumBuilds, SmallBuildingCollection smallBuildings, MainRoad mainRoad, GameFlowService gameFlowService, EventBus eventBus)
    {
        _config = config;
        _buildingPrefabsBysize = new()
        {
            { BuildSize.Large, largeBuildings.Prefabs },
            { BuildSize.Medium, mediumBuilds.Prefabs },
            { BuildSize.Small, smallBuildings.Prefabs }
        };
        _buildSizeCount = Enum.GetNames(typeof(BuildSize)).Length;

        _mainRoadRenderer = mainRoad.GetComponent<MeshRenderer>();
        _gameFlowService = gameFlowService;
        _spawnedBuilds = new();
        eventBus.OnStartRaid += OnStartRaid;
        eventBus.OnStopRaid += OnStopRaid;
    }

    public void OnStartRaid()
    {
        if (ctsOnStopRaid != null)
        {
            ctsOnStopRaid.Dispose();
        }
        ctsOnStopRaid = new CancellationTokenSource();

        _gameFlowService.CustomUpdate += CustomUpdate;
        SpawnBuildstRecursive(ctsOnStopRaid.Token).Forget();
    }
    public void OnStopRaid()
    {
        ctsOnStopRaid.Cancel();
        ctsOnStopRaid.Dispose();


        _gameFlowService.CustomUpdate -= CustomUpdate;

        foreach (var build in _spawnedBuilds)
        {
            Destroy(build.gameObject);
        }
        _spawnedBuilds.Clear();

    }

    private void CustomUpdate()
    {
        MoveMainRoad();
        MoveBuilds();
    }

    void MoveMainRoad()
    {
        _mainRoadRenderer.material.mainTextureOffset += _config.GroundMoveSpeed * Time.deltaTime * Vector2.left;
    }

    void MoveBuilds()
    {
        for (int i = _spawnedBuilds.Count - 1; i >= 0; i--)
        {
            if (_spawnedBuilds[i].position.x < _config.EnvironmentsAreaZone.XMin)
            {
                Destroy(_spawnedBuilds[i].gameObject);
                _spawnedBuilds.RemoveAt(i);
            }
            else
            {
                _spawnedBuilds[i].Translate(_config.EnvironmentMoveSpeed * Time.deltaTime * Vector3.left, Space.World);
            }
        }
    }

    async UniTaskVoid SpawnBuildstRecursive(CancellationToken ct)
    {
        float delay = Random.Range(_config.LargeObjectSpawnDelayRange.x, _config.LargeObjectSpawnDelayRange.y);
        await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: ct);

        float randomRotateAngle = Random.Range(0f, _config.ObjectsMaxRotationOnSpawn);
        float randomTiltZ = Random.Range(0f, _config.ObjectsMaxTiltOnSpawn);
        float randomTiltX = Random.Range(0f, _config.ObjectsMaxTiltOnSpawn);
        float higher = randomTiltZ > randomTiltX ? randomTiltZ : randomTiltX;

        int randomSizeIndex = Random.Range(0, _buildSizeCount);
        BuildSize buildSize = (BuildSize)randomSizeIndex;
        float correctYPos = 0;

        switch (buildSize)
        {
            case BuildSize.Large:
                correctYPos = higher * _config.LargeObjectsCorectYPosByTiltMod;
                break;
            case BuildSize.Medium:
                correctYPos = higher * _config.MediumObjectsCorectYPosByTiltMod;
                break;
            case BuildSize.Small:
                correctYPos = higher * _config.SmallObjectsCorectYPosByTiltMod;
                break;
        }


        float spawnXPos = _config.EnvironmentsAreaZone.XMax;
        float spawnZPos = Random.Range(_config.EnvironmentsAreaZone.ZMin, (float)_config.EnvironmentsAreaZone.ZMax);

        Vector3 spawnPos = new(spawnXPos, correctYPos, spawnZPos);
        Quaternion newRotation = Quaternion.Euler(randomTiltX, randomRotateAngle, randomTiltZ);

        int randomIndex = Random.Range(0, _buildingPrefabsBysize[buildSize].Length);

        _spawnedBuilds.Add(Instantiate(_buildingPrefabsBysize[buildSize][randomIndex], spawnPos, newRotation));
        SpawnBuildstRecursive(ct).Forget();
    }
}
