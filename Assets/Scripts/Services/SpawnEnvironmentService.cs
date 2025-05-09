using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
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
        SpawnBuilds(ctsOnStopRaid.Token).Forget();
    }

    async UniTaskVoid SpawnBuilds(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            float delay = _config.LargeObjectSpawnRepeatRange.RandomValue();
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: ct);

            ObjectSize objectSize = (ObjectSize)Random.Range(0, _buildSizeCount);

            int randomIndex = Random.Range(0, _buildingPrefabsBysize[objectSize].Length);
            EnvironmentObject prefab = _buildingPrefabsBysize[objectSize][randomIndex];

            var asyncInstantiateOperation = InstantiateAsync(prefab);
            while (!asyncInstantiateOperation.isDone)
            {
                await UniTask.Yield();
            }
            EnvironmentObject spawnedObject = asyncInstantiateOperation.Result.First();

            float randomRotateAngle = Random.Range(0f, _config.ObjectsMaxRotationOnSpawn);
            float randomTiltZ = Random.Range(0f, _config.ObjectsMaxTiltOnSpawn);
            float randomTiltX = Random.Range(0f, _config.ObjectsMaxTiltOnSpawn);
            float higherTilt = randomTiltZ > randomTiltX ? randomTiltZ : randomTiltX;

            float correctYPos = higherTilt;
            correctYPos *= objectSize switch
            {
                ObjectSize.Large => _config.LargeObjectsCorectYPosByTiltMod,
                ObjectSize.Medium => _config.MediumObjectsCorectYPosByTiltMod,
                ObjectSize.Small => _config.SmallObjectsCorectYPosByTiltMod,
                _ => 0
            };

            spawnedObject.transform.rotation = Quaternion.Euler(randomTiltX, randomRotateAngle, randomTiltZ);
            Vector3 correctingPos = GetRandomPosInZoneXZ(_config.EnvironmentsAreaZone, spawnedObject.ObjectRenderer.bounds, SpawnPivot.XMAx);
            correctingPos.y = correctYPos;
            spawnedObject.transform.position = correctingPos;

            _eventBus.OnSpawnEnvironmentObject?.Invoke(spawnedObject);
            _spawnedGameObjects.Add(spawnedObject.gameObject);
        }
    }
}
