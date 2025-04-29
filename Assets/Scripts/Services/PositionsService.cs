using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class PositionsService : MonoBehaviour
{
    List<Vector3> _deffPositions;
    List<Vector3> _freePositions;
    Dictionary<Enemy, Vector3> _reservedPositions;
    EventBus _eventBus;
    Config _config;

    [Inject]
    public void Construct(EventBus eventBus, Config config)
    {
        _eventBus = eventBus;
        _config = config;
        _deffPositions = _config.FightZonePointsPositions;
        _reservedPositions = new();
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
        _eventBus.OnEnemyDie += OnEnemyDie;
        _freePositions = new(_deffPositions);
        _reservedPositions.Clear();
    }
    private void OnStopRaid()
    {
        _eventBus.OnEnemyDie -= OnEnemyDie;
    }

    private void OnEnemyDie(Enemy enemy)
    {
        if (_reservedPositions.TryGetValue(enemy, out Vector3 pos))
        {
            _freePositions.Add(pos);
            _reservedPositions.Remove(enemy);
        }        
    }

    public Vector3 GetFreePosInFightZone(Enemy forWhom, int specificIndex)
    {
        Vector3 pos = _freePositions[specificIndex];
        _freePositions.RemoveAt(specificIndex);
        _reservedPositions.Add(forWhom, pos);
        return pos;
    }

    public Vector3 GetFreePosInFightZone(Enemy forWhom)
    {
        if (_freePositions.Count == 0) return Vector3.zero;

        int rIndex = Random.Range(0, _freePositions.Count);
        Vector3 pos = _freePositions[rIndex];

        if (_reservedPositions.ContainsKey(forWhom))
        {
            _freePositions.Add(_reservedPositions[forWhom]);
            _reservedPositions[forWhom] = pos;
            _freePositions.Remove(pos);
        }
        else
        {
            _freePositions.RemoveAt(rIndex);
            _reservedPositions.Add(forWhom, pos);
        }
        return pos;
    }

    public Vector3 GetPosForBonusEnemy(bool spawnedBehind)
    {
        Vector3 pos;

        float randomZPos = _config.BonusEnemyZone.GetRandomZPos();
        if (spawnedBehind)
        {
            pos = new Vector3(_config.BonusEnemyZone.XMax, 0, randomZPos);
        }
        else
        {
            pos = new Vector3(_config.BonusEnemyZone.XMin, 0, randomZPos);
        }
        return pos;
    }
}
