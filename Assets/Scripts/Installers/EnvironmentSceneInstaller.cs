using UnityEngine;
using Zenject;

public class EnvironmentSceneInstaller : MonoInstaller
{
    [SerializeField] MainRoad _mainRoad;
    [SerializeField] EnvironmentPrefabsCollection _largeBuildings;
    [SerializeField] EnvironmentPrefabsCollection _mediumBuildings;
    [SerializeField] EnvironmentPrefabsCollection _smallBuildings;
    [SerializeField] PositionsService _positionsService;
    [SerializeField] PlayerShootService _playerShootService;
    [SerializeField] PlayerAIMService _playerAIMService;
    [SerializeField] VehiclePartsHP _vehiclePartsHP;
    [SerializeField] EnemyHpService _enemyHpService;
    [SerializeField] DetachService _detachService;
    [SerializeField] HitVisualService _hitVisualService;

    public override void InstallBindings()
    {
        Container.Bind<LargeBuildingCollection>().FromInstance(_largeBuildings as LargeBuildingCollection).AsSingle();
        Container.Bind<MediumBuildingCollection>().FromInstance(_mediumBuildings as MediumBuildingCollection).AsSingle();
        Container.Bind<SmallBuildingCollection>().FromInstance(_smallBuildings as SmallBuildingCollection).AsSingle();
        Container.Bind<MainRoad>().FromInstance(_mainRoad).AsSingle();
        Container.Bind<PositionsService>().FromInstance(_positionsService).AsSingle();
        Container.Bind<PlayerShootService>().FromInstance(_playerShootService).AsSingle();
        Container.Bind<PlayerAIMService>().FromInstance(_playerAIMService).AsSingle();
        Container.Bind<VehiclePartsHP>().FromInstance(_vehiclePartsHP).AsSingle();
        Container.Bind<EnemyHpService>().FromInstance(_enemyHpService).AsSingle();
        Container.Bind<DetachService>().FromInstance(_detachService).AsSingle();
        Container.Bind<HitVisualService>().FromInstance(_hitVisualService).AsSingle();
    }
}