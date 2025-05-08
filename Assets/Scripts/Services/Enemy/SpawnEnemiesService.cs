using Cysharp.Threading.Tasks;
using System;
using System.Linq;
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
        SpawnFightingEnemies(ctsOnStopRaid.Token).Forget();
        SpawnBonusEnemies(ctsOnStopRaid.Token).Forget();
    }
    protected override void OnStopRaid()
    {
        _eventBus.OnChangeEnemiesTir -= OnChangeEnemiesTir;
        base.OnStopRaid();
    }

    private void OnChangeEnemiesTir(int newTir)
    {
        _currentTirIndex = newTir - 1;
        if (newTir >= _enemiesCollections.Length)
        {
            _currentTirIndex = _enemiesCollections.Length - 1;
        }
    }

    async UniTaskVoid SpawnFightingEnemies(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            float delay = _config.SpawnFighingEnemyRepeatRange.RandomValue();
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: ct);
            if (_spawnedGameObjects.Count >= _config.MaxFighingEnemiesCount)
            {
                continue;
            }

            int randomIndex = Random.Range(0, _enemiesCollections[_currentTirIndex].FightingEnemies.Length);
            FightingEnemy prefab = _enemiesCollections[_currentTirIndex].FightingEnemies[randomIndex];

            bool leftZone = Random.Range(0, 1f) < 0.5f;
            AreaZone spawnZone = leftZone ? _config.SpawnEnemiesZone_Left : _config.SpawnEnemiesZone_Right;
            SpawnPivot spawnPivot = leftZone ? SpawnPivot.Xmin : SpawnPivot.XMAx;

            Vector3 spawnPos = GetRandomPosInZoneXZ(spawnZone, prefab.CombinedBounds, spawnPivot);
            //FightingEnemy spawnedObject = _container.InstantiatePrefabForComponent<FightingEnemy>(prefab, spawnPos, prefab.transform.rotation, null);

            var asyncInstantiateOperation = InstantiateAsync(prefab, spawnPos, prefab.transform.rotation);
            while (!asyncInstantiateOperation.isDone)
            {
                await UniTask.Yield();
            }

            FightingEnemy spawnedObject = asyncInstantiateOperation.Result.First();
            _container.Inject(spawnedObject);




            _spawnedGameObjects.Add(spawnedObject.gameObject);
            _eventBus.OnSpawnEnemy?.Invoke(spawnedObject);
        }
    }

    async UniTaskVoid SpawnBonusEnemies(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            float delay = _config.SpawnBonusEnemyRepeatRange.RandomValue();
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: ct);
            if (_spawnedBonusEnemy != null)
            {
                continue;
            }
            if(_enemiesCollections[_currentTirIndex].BonusEnemy == null) continue;
            BonusEnemy prefab = _enemiesCollections[_currentTirIndex].BonusEnemy;
            Vector3 spawnPos = GetRandomPosInZoneXZ(_config.BonusEnemyZone, prefab.CombinedBounds, SpawnPivot.Xmin);

            //BonusEnemy spawnedObject = _container.InstantiatePrefabForComponent<BonusEnemy>(prefab, spawnPos, prefab.transform.rotation, null);
            var asyncInstantiateOperation = InstantiateAsync(prefab, spawnPos, prefab.transform.rotation);
            while (!asyncInstantiateOperation.isDone)
            {
                await UniTask.Yield();
            }

            BonusEnemy spawnedObject = asyncInstantiateOperation.Result.First();
            _container.Inject(spawnedObject);

            _eventBus.OnSpawnEnemy?.Invoke(spawnedObject);
            _spawnedBonusEnemy = spawnedObject;
        }
    }
}
