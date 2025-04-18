using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class SpawnEnemiesService : AbstractSpawnService
{
    EnemiesCollection[] _enemiesCollections;
    DiContainer _container;

    BonusEnemy _spawnedBonusEnemy;

    int _currentTirIndex;

    [Inject]
    public void Construct(EnemiesCollection[] enemiesCollections, DiContainer diContainer)
    {
        _enemiesCollections = enemiesCollections;
        _container = diContainer;
    }

    protected override void OnStartRaid()
    {
        _currentTirIndex = 0;
        _eventBus.OnChangeEnemiesTir += OnChangeEnemiesTir;
        base.OnStartRaid();
        SpawnFightingEnemiesRecursive(ctsOnStopRaid.Token).Forget();
        SpawnBonusEnemiesRecursive(ctsOnStopRaid.Token).Forget();
    }
    protected override void OnStopRaid()
    {
        _eventBus.OnChangeEnemiesTir -= OnChangeEnemiesTir;
        base.OnStopRaid();
    }

    private void OnChangeEnemiesTir(int newTir)
    {
        _currentTirIndex = newTir - 1;
    }

    async UniTaskVoid SpawnFightingEnemiesRecursive(CancellationToken ct)
    {
        float delay = _config.SpawnFighingEnemyRepeatRange.RandomValue();
        await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: ct);
        if (_spawnedGameObjects.Count >= _config.MaxFighingEnemiesCount)
        {
            SpawnFightingEnemiesRecursive(ct).Forget();
            return;
        }

        int randomIndex = Random.Range(0, _enemiesCollections[_currentTirIndex].FightingEnemies.Length);
        FightingEnemy prefab = _enemiesCollections[_currentTirIndex].FightingEnemies[randomIndex];

        bool leftZone = Random.Range(0, 1f) < 0.5f;
        AreaZone spawnZone = leftZone ? _config.SpawnEnemiesZone_Left : _config.SpawnEnemiesZone_Right;
        SpawnPivot spawnPivot = leftZone ? SpawnPivot.Xmin : SpawnPivot.XMAx;

        Vector3 spawnPos = GetRandomPosInZoneXZ(spawnZone, prefab.CombinedBounds, spawnPivot);
        FightingEnemy spawnedObject = _container.InstantiatePrefabForComponent<FightingEnemy>(prefab, spawnPos, prefab.transform.rotation, null);
        _spawnedGameObjects.Add(spawnedObject.gameObject);
        _eventBus.OnSpawnEnemy?.Invoke(spawnedObject);
        SpawnFightingEnemiesRecursive(ct).Forget();
    }

    async UniTaskVoid SpawnBonusEnemiesRecursive(CancellationToken ct)
    {
        float delay = _config.SpawnBonusEnemyRepeatRange.RandomValue();
        await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: ct);
        if (_spawnedBonusEnemy != null)
        {
            SpawnFightingEnemiesRecursive(ct).Forget();
            return;
        }
        BonusEnemy prefab = _enemiesCollections[_currentTirIndex].BonusEnemy;
        Vector3 spawnPos = GetRandomPosInZoneXZ(_config.BonusEnemyZone, prefab.CombinedBounds, SpawnPivot.Xmin);

        BonusEnemy spawnedObject = _container.InstantiatePrefabForComponent<BonusEnemy>(prefab, spawnPos, prefab.transform.rotation, null);
        _eventBus.OnSpawnEnemy?.Invoke(spawnedObject);
        _spawnedBonusEnemy = spawnedObject;
        SpawnBonusEnemiesRecursive(ct).Forget();
    }
}
