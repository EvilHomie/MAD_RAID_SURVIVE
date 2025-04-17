using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public abstract class AbstractSpawnService : MonoBehaviour
{
    [SerializeField] float _bordersOffset;

    protected Config _config;
    protected CancellationTokenSource ctsOnStopRaid;
    protected EventBus _eventBus;

    protected List<GameObject> _spawnedGameObjects;

    [Inject]
    public void Construct(Config config, EventBus eventBus)
    {
        _config = config;
        _eventBus = eventBus;
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

    protected virtual void OnStartRaid()
    {
        ctsOnStopRaid?.Dispose();
        ctsOnStopRaid = new CancellationTokenSource();
    }
    protected virtual void OnStopRaid()
    {
        foreach (GameObject gameObject in _spawnedGameObjects)
        {
            Destroy(gameObject.gameObject);
        }
        _spawnedGameObjects.Clear();
        ctsOnStopRaid.Cancel();
        ctsOnStopRaid.Dispose();
    }

    Vector3 ClampObjectInAreaBorderXZ(AreaZone areaZone, Bounds objectBounds, Vector3 spawnPos)
    {
        if (spawnPos.x + objectBounds.extents.x > areaZone.XMax - _bordersOffset)
        {
            spawnPos.x = areaZone.XMax - _bordersOffset - objectBounds.extents.x;
        }
        else if (spawnPos.x - objectBounds.extents.x < areaZone.XMin + _bordersOffset)
        {
            spawnPos.x = areaZone.XMin + _bordersOffset + objectBounds.extents.x;
        }

        if (spawnPos.z + objectBounds.extents.z > areaZone.ZMax - _bordersOffset)
        {
            spawnPos.z = areaZone.ZMax - _bordersOffset - objectBounds.extents.z;
        }
        else if (spawnPos.z - objectBounds.extents.z < areaZone.ZMin + _bordersOffset)
        {
            spawnPos.z = areaZone.ZMin + _bordersOffset + objectBounds.extents.z;
        }
        return spawnPos;
    }

    protected Vector3 GetRandomPosInZoneXZ(AreaZone areaZone, Bounds objectBounds, SpawnPivot spawnPivot)
    {
        Vector3 randomPos = spawnPivot switch
        {
            SpawnPivot.None => new Vector3(Random.Range(areaZone.XMin, areaZone.XMax), 0, Random.Range(areaZone.ZMin, areaZone.ZMax)),
            SpawnPivot.XMAx => new Vector3(areaZone.XMax, 0, Random.Range(areaZone.ZMin, areaZone.ZMax)),
            SpawnPivot.Xmin => new Vector3(areaZone.XMin, 0, Random.Range(areaZone.ZMin, areaZone.ZMax)),
            SpawnPivot.ZMAx => new Vector3(Random.Range(areaZone.XMin, areaZone.XMax), 0, areaZone.ZMax),
            SpawnPivot.Zmin => new Vector3(Random.Range(areaZone.XMin, areaZone.XMax), 0, areaZone.ZMin),
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