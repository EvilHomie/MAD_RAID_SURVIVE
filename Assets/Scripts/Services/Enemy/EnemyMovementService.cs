using Cysharp.Threading.Tasks;
using Pathfinding;
using Pathfinding.ECS;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class EnemyMovementService : AbstractInRaidService
{
    PositionsService _positionsService;

    List<BonusEnemy> _bonusEnemies;
    List<FightingEnemy> _fightingEnemiesNotReachedFightPoint;
    List<FightingEnemy> _fightingEnemiesReachedFightPoint;
    List<Enemy> _deadEnemies;

    CancellationTokenSource _ctsOnStopRaid;

    [Inject]
    public void Construct(PositionsService positionsService)
    {
        _positionsService = positionsService;
        _bonusEnemies = new();
        _fightingEnemiesNotReachedFightPoint = new();
        _fightingEnemiesReachedFightPoint = new();
        _deadEnemies = new();
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
    protected override void OnStartRaid()
    {
        _eventBus.OnSpawnEnemy += OnEnemySpawned;
        _eventBus.OnEnemyDie += OnEnemyDie;
        _gameFlowService.CustomFixedUpdate += CustomFixedUpdate;

        _ctsOnStopRaid = _ctsOnStopRaid.Create();
        CheckReachedFightPoint(_ctsOnStopRaid.Token).Forget();
    }
    protected override void OnStopRaid()
    {
        _eventBus.OnSpawnEnemy -= OnEnemySpawned;
        _eventBus.OnEnemyDie -= OnEnemyDie;
        _gameFlowService.CustomFixedUpdate -= CustomFixedUpdate;

        _bonusEnemies.Clear();
        _fightingEnemiesNotReachedFightPoint.Clear();
        _fightingEnemiesReachedFightPoint.Clear();
        _deadEnemies.Clear();

        _ctsOnStopRaid.CancelAndDispose();
    }

    private void CustomFixedUpdate()
    {
        MoveOnDie();
        ChangePosInFightZone();
    }

    private void OnEnemySpawned(Enemy enemy)
    {
        enemy.NavmeshCut.enabled = false;
        enemy.Rigidbody.interpolation = RigidbodyInterpolation.None;
        if (enemy is FightingEnemy fightingEnemy)
        {
            _fightingEnemiesNotReachedFightPoint.Add(fightingEnemy);
            fightingEnemy._pivotPosInFightZone = _positionsService.GetFreePosInFightZone(fightingEnemy);
            fightingEnemy.IAstarAI.maxSpeed = _config.EnemySpeedOutOfFightZone;
            fightingEnemy.IAstarAI.destination = fightingEnemy._pivotPosInFightZone;

            ChangeSlowDownTime(fightingEnemy.IAstarAI, _config.SlowDownTimeOutOfFightZone);


        }
        else if (enemy is BonusEnemy bonusEnemy)
        {
            _bonusEnemies.Add(bonusEnemy);
            
            bool spawnedBehind = enemy.transform.position.x <= 0;
            bonusEnemy.IAstarAI.maxSpeed = _config.BonusEnemySpeed;
            bonusEnemy.IAstarAI.destination = _positionsService.GetPosForBonusEnemy(spawnedBehind);

        }
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
                    ChangeSlowDownTime(_fightingEnemiesNotReachedFightPoint[i].IAstarAI, _config.SlowDownTimeInFightZone);

                    _fightingEnemiesReachedFightPoint.Add(_fightingEnemiesNotReachedFightPoint[i]);
                    _fightingEnemiesNotReachedFightPoint.Remove(_fightingEnemiesNotReachedFightPoint[i]);
                }
            }
        }
    }

    void ChangePosInFightZone()
    {
        if (_fightingEnemiesReachedFightPoint.Count == 0) return;
        for (int i = _fightingEnemiesReachedFightPoint.Count - 1; i >= 0; i--)
        {
            FightingEnemy enemy = _fightingEnemiesReachedFightPoint[i];
            if (enemy._isDead)
            {
                _fightingEnemiesReachedFightPoint.RemoveAt(i);
                continue;
            }


            if (enemy._changePosInFightZoneRemainingTime > 0)
            {
                enemy._changePosInFightZoneRemainingTime -= Time.fixedDeltaTime;
                if (enemy._changePosInFightZoneRemainingTime < 0)
                {
                    Vector3 newPos = _positionsService.GetFreePosInFightZone(enemy);
                    if (newPos == Vector3.zero)
                    {
                        enemy._changePosInFightZoneRemainingTime = Random.Range(_config.ChangePosInFightZoneRepeatRange.x, _config.ChangePosInFightZoneRepeatRange.y);
                    }
                    else
                    {
                        newPos.y = 0;
                        enemy.IAstarAI.destination = newPos;
                        enemy._changePosInFightZoneRemainingTime = Random.Range(_config.ChangePosInFightZoneRepeatRange.x, _config.ChangePosInFightZoneRepeatRange.y);

                        _fightingEnemiesNotReachedFightPoint.Add(enemy);
                        _fightingEnemiesReachedFightPoint.Remove(enemy);
                    }
                }
            }
            else if (enemy._changePosInFightZoneRemainingTime == 0)
            {
                enemy._changePosInFightZoneRemainingTime = Random.Range(_config.ChangePosInFightZoneRepeatRange.x, _config.ChangePosInFightZoneRepeatRange.y);
            }
        }
    }

    private void OnEnemyDie(Enemy enemy)
    {
        enemy.IAstarAI.enabled = false;
        enemy.NavmeshCut.enabled = true;
        enemy.Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        enemy.Rigidbody.maxLinearVelocity = _config.OnDieTranslationMaxSpeed;
        _deadEnemies.Add(enemy);
    }

    void MoveOnDie()
    {
        if (_deadEnemies.Count == 0) return;
        for (int i = _deadEnemies.Count - 1; i >= 0; i--)
        {
            if (_deadEnemies[i] == null)
            {
                _deadEnemies.RemoveAt(i);
                continue;
            }
            _deadEnemies[i].Rigidbody.AddForce(_config.OnDieAccelerationSpeed * Vector3.left, ForceMode.Acceleration);
        }
    }

    void ChangeSlowDownTime(FollowerEntity follower, float time)
    {
        MovementSettings movementSettings = follower.movementSettings;
        movementSettings.follower.slowdownTime = time;
        follower.movementSettings = movementSettings;
    }
}



//void SimulateFloatingPosInFightZone()
//{
//    foreach (var enemy in _fightingEnemiesReachedFightPoint)
//    {
//        if (enemy._simulateFloatingPosRemainingTime > 0)
//        {
//            enemy._simulateFloatingPosRemainingTime -= Time.deltaTime;
//            if (enemy._simulateFloatingPosRemainingTime < 0)
//            {
//                Vector3 newFloatingPos = enemy._pivotPosInFightZone + Random.insideUnitSphere * _config.SimulateFloatingPosRadius;
//                newFloatingPos.y = 0;
//                enemy.IAstarAI.destination = newFloatingPos;
//                enemy._simulateFloatingPosRemainingTime = Random.Range(_config.SimulateFloatingPosRepeatRange.x, _config.SimulateFloatingPosRepeatRange.y);
//            }
//        }
//        else if (enemy._simulateFloatingPosRemainingTime == 0)
//        {
//            enemy._simulateFloatingPosRemainingTime = Random.Range(_config.SimulateFloatingPosRepeatRange.x, _config.SimulateFloatingPosRepeatRange.y);
//        }
//    }
//}
