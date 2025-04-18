using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class EnemyMovementService : MonoBehaviour
{
    EventBus _eventBus;
    Config _config;
    GameFlowService _gameFlowService;
    PositionsService _positionsService;

    List<BonusEnemy> _bonusEnemies;
    List<FightingEnemy> _fightingEnemiesNotReachedFightPoint;
    List<FightingEnemy> _fightingEnemiesReachedFightPoint;


    CancellationTokenSource _ctsOnStopRaid;

    [Inject]
    public void Construct(Config config, GameFlowService gameFlowService, EventBus eventBus, PositionsService positionsService)
    {
        _config = config;
        _gameFlowService = gameFlowService;
        _eventBus = eventBus;
        _positionsService = positionsService;
        _bonusEnemies = new();
        _fightingEnemiesNotReachedFightPoint = new();
        _fightingEnemiesReachedFightPoint = new();
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
        _eventBus.OnSpawnEnemy += OnEnemySpawned;
        _eventBus.OnEnemyDie += OnEnemyDie;
        _gameFlowService.CustomUpdate += CustomUpdate;

        _ctsOnStopRaid?.Dispose();
        _ctsOnStopRaid = new CancellationTokenSource();
        CheckReachedFightPoint(_ctsOnStopRaid.Token).Forget();
    }
    private void OnStopRaid()
    {
        _eventBus.OnSpawnEnemy -= OnEnemySpawned;
        _eventBus.OnEnemyDie -= OnEnemyDie;
        _gameFlowService.CustomUpdate -= CustomUpdate;

        _bonusEnemies.Clear();
        _fightingEnemiesNotReachedFightPoint.Clear();
        _fightingEnemiesReachedFightPoint.Clear();

        _ctsOnStopRaid.Cancel();
        _ctsOnStopRaid.Dispose();
    }
    private void CustomUpdate()
    {
        //SimulateFloatingPosInFightZone();
        ChangePosInFightZone();
    }

    private void OnEnemySpawned(Enemy enemy)
    {
        if (enemy is FightingEnemy fightingEnemy)
        {
            _fightingEnemiesNotReachedFightPoint.Add(fightingEnemy);
            fightingEnemy.pivotPosInFightZone = _positionsService.GetFreePosInFightZone(fightingEnemy);
            fightingEnemy.IAstarAI.maxSpeed = _config.EnemySpeedOutOfFightZone;
            fightingEnemy.IAstarAI.destination = fightingEnemy.pivotPosInFightZone;
        }
        else if (enemy is BonusEnemy bonusEnemy)
        {
            _bonusEnemies.Add(bonusEnemy);
            bool spawnedBehind = enemy.transform.position.x <= 0;
            bonusEnemy.IAstarAI.destination = _positionsService.GetPosForBonusEnemy(spawnedBehind);
        }
    }
    private void OnEnemyDie(Enemy enemy)
    {
        throw new NotImplementedException();
    }


    async UniTaskVoid CheckReachedFightPoint(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_config.CheckReachedEndOfPathRepeatDelay), ignoreTimeScale: false, cancellationToken: ct);
            if (_fightingEnemiesNotReachedFightPoint.Count == 0) continue;

            for (int i = _fightingEnemiesNotReachedFightPoint.Count - 1; i >= 0; i--)
            {
                if (_fightingEnemiesNotReachedFightPoint[i].IAstarAI.reachedEndOfPath)
                {
                    _fightingEnemiesNotReachedFightPoint[i].IAstarAI.maxSpeed = _config.EnemySpeedInsideFightZone;
                    _fightingEnemiesReachedFightPoint.Add(_fightingEnemiesNotReachedFightPoint[i]);
                    _fightingEnemiesNotReachedFightPoint.Remove(_fightingEnemiesNotReachedFightPoint[i]);
                }
            }
        }
    }

    void SimulateFloatingPosInFightZone()
    {
        foreach (var enemy in _fightingEnemiesReachedFightPoint)
        {
            if (enemy.simulateFloatingPosRemainingTime > 0)
            {
                enemy.simulateFloatingPosRemainingTime -= Time.deltaTime;
                if (enemy.simulateFloatingPosRemainingTime < 0)
                {
                    Vector3 newFloatingPos = enemy.pivotPosInFightZone + Random.insideUnitSphere * _config.SimulateFloatingPosRadius;
                    newFloatingPos.y = 0;
                    enemy.IAstarAI.destination = newFloatingPos;
                    enemy.simulateFloatingPosRemainingTime = Random.Range(_config.SimulateFloatingPosRepeatRange.x, _config.SimulateFloatingPosRepeatRange.y);
                }
            }
            else if (enemy.simulateFloatingPosRemainingTime == 0)
            {
                enemy.simulateFloatingPosRemainingTime = Random.Range(_config.SimulateFloatingPosRepeatRange.x, _config.SimulateFloatingPosRepeatRange.y);
            }
        }
    }

    void ChangePosInFightZone()
    {
        if (_fightingEnemiesReachedFightPoint.Count == 0) return;
        for (int i = _fightingEnemiesReachedFightPoint.Count - 1; i >= 0; i--)
        {
            FightingEnemy enemy = _fightingEnemiesReachedFightPoint[i];

            if (enemy.changePosInFightZoneRemainingTime > 0)
            {
                enemy.changePosInFightZoneRemainingTime -= Time.deltaTime;
                if (enemy.changePosInFightZoneRemainingTime < 0)
                {
                    Vector3 newPos = _positionsService.GetFreePosInFightZone(enemy);
                    if (newPos == Vector3.zero)
                    {
                        enemy.changePosInFightZoneRemainingTime = Random.Range(_config.ChangePosInFightZoneRepeatRange.x, _config.ChangePosInFightZoneRepeatRange.y);
                    }
                    else
                    {
                        newPos.y = 0;
                        enemy.IAstarAI.destination = newPos;
                        enemy.changePosInFightZoneRemainingTime = Random.Range(_config.ChangePosInFightZoneRepeatRange.x, _config.ChangePosInFightZoneRepeatRange.y);

                        _fightingEnemiesNotReachedFightPoint.Add(enemy);
                        _fightingEnemiesReachedFightPoint.Remove(enemy);
                    }
                }
            }
            else if (enemy.changePosInFightZoneRemainingTime == 0)
            {
                enemy.changePosInFightZoneRemainingTime = Random.Range(_config.ChangePosInFightZoneRepeatRange.x, _config.ChangePosInFightZoneRepeatRange.y);
            }
        }
    }

    void MoveOnDie()
    {

    }
}
