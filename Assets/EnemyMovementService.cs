using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;
using static UnityEngine.EventSystems.EventTrigger;
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
    }

    private void OnEnable()
    {
        _eventBus.OnStartRaid += OnStartRaid;
        _eventBus.OnStopRaid += OnStopRaid;
        _eventBus.OnEnemySpawned += OnEnemySpawned;
        _eventBus.OnEnemyDie += OnEnemyDie;
        _gameFlowService.CustomUpdate += CustomUpdate;
    }

    private void CustomUpdate()
    {
        SimulateFloatingPosInFightZone();
    }

    private void OnDisable()
    {
        _eventBus.OnStartRaid -= OnStartRaid;
        _eventBus.OnStopRaid -= OnStopRaid;
        _eventBus.OnEnemySpawned -= OnEnemySpawned;
        _eventBus.OnEnemyDie -= OnEnemyDie;
        _gameFlowService.CustomUpdate -= CustomUpdate;
    }

    private void OnStartRaid()
    {
        if (_ctsOnStopRaid != null)
        {
            _ctsOnStopRaid.Dispose();
        }
        _ctsOnStopRaid = new CancellationTokenSource();
        CheckReachedEndOfPath(_ctsOnStopRaid.Token).Forget();
    }
    private void OnStopRaid()
    {
        _ctsOnStopRaid.Cancel();
        _ctsOnStopRaid.Dispose();
    }

    private void OnEnemySpawned(Enemy enemy)
    {
        if (enemy is FightingEnemy fightingEnemy)
        {
            _fightingEnemiesNotReachedFightPoint.Add(fightingEnemy);
            fightingEnemy.pivotPosInFightZone = _positionsService.GetFreePosInFightZone(fightingEnemy);
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


    async UniTaskVoid CheckReachedEndOfPath(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_config.CheckReachedEndOfPath), ignoreTimeScale: false, cancellationToken: ct);

            foreach (var enemy in _fightingEnemiesNotReachedFightPoint)
            {
                if (enemy.IAstarAI.reachedEndOfPath)
                {
                    _fightingEnemiesNotReachedFightPoint.Remove(enemy);
                    _fightingEnemiesReachedFightPoint.Add(enemy);
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
                    enemy.simulateFloatingPosRemainingTime = Random.Range(_config.SimulateFloatingPosMinMaxDelay.x, _config.SimulateFloatingPosMinMaxDelay.y);
                }
            }
            else if (enemy.simulateFloatingPosRemainingTime == 0)
            {
                enemy.simulateFloatingPosRemainingTime = Random.Range(_config.SimulateFloatingPosMinMaxDelay.x, _config.SimulateFloatingPosMinMaxDelay.y);
            }

        }
    }
}
