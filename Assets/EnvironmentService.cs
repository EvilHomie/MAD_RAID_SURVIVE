using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;


public class EnvironmentService : MonoBehaviour
{
    Dictionary<ObjectSize, Renderer[]> _buildingPrefabsBysize;
    Config _config;
    MeshRenderer _mainRoadRenderer;
    GameFlowService _gameFlowService;
    List<Renderer> _spawnedBuilds;
    CancellationTokenSource ctsOnStopRaid;

    int _buildSizeCount;


    [Inject]
    public void Construct(Config config, LargeBuildingCollection largeBuildings, MediumBuildingCollection mediumBuilds, SmallBuildingCollection smallBuildings, MainRoad mainRoad, GameFlowService gameFlowService, EventBus eventBus)
    {
        _config = config;
        _buildingPrefabsBysize = new()
        {
            { ObjectSize.Large, largeBuildings.Prefabs },
            { ObjectSize.Medium, mediumBuilds.Prefabs },
            { ObjectSize.Small, smallBuildings.Prefabs }
        };
        _buildSizeCount = Enum.GetNames(typeof(ObjectSize)).Length;

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

    async UniTaskVoid SpawnBuildstRecursive(CancellationToken ct)
    {
        float delay = Random.Range(_config.LargeObjectSpawnDelayRange.x, _config.LargeObjectSpawnDelayRange.y);
        await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: ct);

        float randomRotateAngle = Random.Range(0f, _config.ObjectsMaxRotationOnSpawn);
        float randomTiltZ = Random.Range(0f, _config.ObjectsMaxTiltOnSpawn);
        float randomTiltX = Random.Range(0f, _config.ObjectsMaxTiltOnSpawn);
        float higher = randomTiltZ > randomTiltX ? randomTiltZ : randomTiltX;

        int randomSizeIndex = Random.Range(0, _buildSizeCount);
        ObjectSize objectSize = (ObjectSize)randomSizeIndex;

        int randomIndex = Random.Range(0, _buildingPrefabsBysize[objectSize].Length);
        Renderer spawnObject = _buildingPrefabsBysize[objectSize][randomIndex];

        float correctYPos = 0;

        switch (objectSize)
        {
            case ObjectSize.Large:
                correctYPos = higher * _config.LargeObjectsCorectYPosByTiltMod;
                break;
            case ObjectSize.Medium:
                correctYPos = higher * _config.MediumObjectsCorectYPosByTiltMod;
                break;
            case ObjectSize.Small:
                correctYPos = higher * _config.SmallObjectsCorectYPosByTiltMod;
                break;
        }

        Quaternion newRotation = Quaternion.Euler(randomTiltX, randomRotateAngle, randomTiltZ);

        spawnObject.transform.rotation = newRotation;

        float objectBoundRadiusX = spawnObject.bounds.max.x - spawnObject.bounds.center.x;
        float objectBoundRadiusZ = spawnObject.bounds.max.z - spawnObject.bounds.center.z;

        float spawnXPos = _config.EnvironmentsAreaZone.XMax - objectBoundRadiusX;
        float spawnZPos = Random.Range(_config.EnvironmentsAreaZone.ZMin, (float)_config.EnvironmentsAreaZone.ZMax);

        if (spawnZPos + objectBoundRadiusZ > _config.EnvironmentsAreaZone.ZMax)
        {
            spawnZPos = _config.EnvironmentsAreaZone.ZMax - objectBoundRadiusZ;
        }
        else if (spawnZPos - objectBoundRadiusZ < _config.EnvironmentsAreaZone.ZMin)
        {
            spawnZPos = _config.EnvironmentsAreaZone.ZMin + objectBoundRadiusZ;
        }

        Vector3 spawnPos = new(spawnXPos, correctYPos, spawnZPos);
        


        _spawnedBuilds.Add(Instantiate(_buildingPrefabsBysize[objectSize][randomIndex], spawnPos, newRotation));
        SpawnBuildstRecursive(ct).Forget();
    }
}
