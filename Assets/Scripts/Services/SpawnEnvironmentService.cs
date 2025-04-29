using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class SpawnEnvironmentService : AbstractSpawnService
{
    Dictionary<ObjectSize, EnvironmentObject[]> _buildingPrefabsBysize;
    int _buildSizeCount;

    [Inject]
    public void Construct(LargeBuildingCollection largeBuildings, MediumBuildingCollection mediumBuilds, SmallBuildingCollection smallBuildings)
    {
        _buildingPrefabsBysize = new()
        {
            { ObjectSize.Large, largeBuildings.Prefabs },
            { ObjectSize.Medium, mediumBuilds.Prefabs },
            { ObjectSize.Small, smallBuildings.Prefabs }
        };
        _buildSizeCount = Enum.GetNames(typeof(ObjectSize)).Length;
    }

    protected override void OnStartRaid()
    {
        base.OnStartRaid();
        SpawnBuildstRecursive(ctsOnStopRaid.Token).Forget();
    }

    async UniTaskVoid SpawnBuildstRecursive(CancellationToken ct)
    {
        float delay = _config.LargeObjectSpawnRepeatRange.RandomValue();
        await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: ct);

        float randomRotateAngle = Random.Range(0f, _config.ObjectsMaxRotationOnSpawn);
        float randomTiltZ = Random.Range(0f, _config.ObjectsMaxTiltOnSpawn);
        float randomTiltX = Random.Range(0f, _config.ObjectsMaxTiltOnSpawn);
        float higherTilt = randomTiltZ > randomTiltX ? randomTiltZ : randomTiltX;

        ObjectSize objectSize = (ObjectSize)Random.Range(0, _buildSizeCount);

        int randomIndex = Random.Range(0, _buildingPrefabsBysize[objectSize].Length);
        EnvironmentObject prefab = _buildingPrefabsBysize[objectSize][randomIndex];

        float correctYPos = higherTilt;

        correctYPos *= objectSize switch
        {
            ObjectSize.Large => _config.LargeObjectsCorectYPosByTiltMod,
            ObjectSize.Medium => _config.MediumObjectsCorectYPosByTiltMod,
            ObjectSize.Small => _config.SmallObjectsCorectYPosByTiltMod,
            _ => 0
        };

        Quaternion newRotation = Quaternion.Euler(randomTiltX, randomRotateAngle, randomTiltZ);

        prefab.transform.rotation = newRotation;
        Vector3 spawnPos = GetRandomPosInZoneXZ(_config.EnvironmentsAreaZone, prefab.objectRenderer.bounds, SpawnPivot.XMAx);
        spawnPos.y = correctYPos;

        EnvironmentObject spawnedObject = Instantiate(prefab, spawnPos, newRotation);
        _eventBus.OnSpawnEnvironmentObject?.Invoke(spawnedObject);
        _spawnedGameObjects.Add(spawnedObject.gameObject);
        SpawnBuildstRecursive(ct).Forget();
    }
}
