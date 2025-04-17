using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class PositionsService : MonoBehaviour
{
    AreaZone _bonusEnemyArea;
    AreaZone _spawnEnemiesArea_Left;
    AreaZone _spawnEnemiesArea_Right;

    List<Vector3> _deffPositions;
    List<Vector3> _freePositions;
    Dictionary<Enemy, Vector3> _reservedPositions;
    EventBus _eventBus;

    [Inject]
    public void Construct(EventBus eventBus, Config config)
    {
        _eventBus = eventBus;
        _deffPositions = config.FightZonePointsPositions;
        _reservedPositions = new();
        _bonusEnemyArea = config.BonusEnemyZone;
        _spawnEnemiesArea_Left = config.SpawnEnemiesZone_Left;
        _spawnEnemiesArea_Right = config.SpawnEnemiesZone_Right;
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
        Vector3 pos = _reservedPositions[enemy];
        _reservedPositions.Remove(enemy);
        _freePositions.Add(pos);
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
        return Vector3.zero;
    }


}
