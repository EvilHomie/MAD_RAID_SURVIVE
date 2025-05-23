using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public abstract class AbstractSpawnService : AbstractInRaidService
{
    

    protected CancellationTokenSource ctsOnStopRaid;

    protected List<GameObject> _spawnedGameObjects;

    [Inject]
    public void Construct()
    {
        _spawnedGameObjects = new();
    }

    void OnEnable()
    {
        _eventBus.OnStartRaid += OnStartRaid;
        _eventBus.OnStopRaid += OnStopRaid;
    }
    void OnDisable()
    {
        _eventBus.OnStartRaid -= OnStartRaid;
        _eventBus.OnStopRaid -= OnStopRaid;
    }

    protected override void OnStartRaid()
    {
        ctsOnStopRaid = ctsOnStopRaid.Create();
    }
    protected override void OnStopRaid()
    {
        foreach (GameObject gameObject in _spawnedGameObjects)
        {
            Destroy(gameObject.gameObject);
        }
        _spawnedGameObjects.Clear();
        ctsOnStopRaid.CancelAndDispose();
    }

    Vector3 ClampObjectInAreaBorderXZ(AreaZone areaZone, Bounds objectBounds, Vector3 spawnPos)
    {
        if (spawnPos.x + objectBounds.extents.x > areaZone.XMax - _config.BordersOffset)
        {
            spawnPos.x = areaZone.XMax - _config.BordersOffset - objectBounds.extents.x;
        }
        else if (spawnPos.x - objectBounds.extents.x < areaZone.XMin + _config.BordersOffset)
        {
            spawnPos.x = areaZone.XMin + _config.BordersOffset + objectBounds.extents.x;
        }

        if (spawnPos.z + objectBounds.extents.z > areaZone.ZMax - _config.BordersOffset)
        {
            spawnPos.z = areaZone.ZMax - _config.BordersOffset - objectBounds.extents.z;
        }
        else if (spawnPos.z - objectBounds.extents.z < areaZone.ZMin + _config.BordersOffset)
        {
            spawnPos.z = areaZone.ZMin + _config.BordersOffset + objectBounds.extents.z;
        }
        return spawnPos;
    }

    protected Vector3 GetRandomPosInZoneXZ(AreaZone areaZone, Bounds objectBounds, SpawnPivot spawnPivot)
    {
        Vector3 randomPos = spawnPivot switch
        {
            SpawnPivot.None => new Vector3(areaZone.GetRandomXPos(), 0, areaZone.GetRandomZPos()),
            SpawnPivot.XMAx => new Vector3(areaZone.XMax, 0, areaZone.GetRandomZPos()),
            SpawnPivot.Xmin => new Vector3(areaZone.XMin, 0, areaZone.GetRandomZPos()),
            SpawnPivot.ZMAx => new Vector3(areaZone.GetRandomXPos(), 0, areaZone.ZMax),
            SpawnPivot.Zmin => new Vector3(areaZone.GetRandomXPos(), 0, areaZone.ZMin),
            _ => Vector3.zero
        };
        return ClampObjectInAreaBorderXZ(areaZone, objectBounds, randomPos);
    }
}

public enum SpawnPivot
{
    None,
    XMAx,
    Xmin,
    ZMAx,
    Zmin
}