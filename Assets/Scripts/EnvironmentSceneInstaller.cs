using UnityEngine;
using Zenject;

public class EnvironmentSceneInstaller : MonoInstaller
{
    [SerializeField] MainRoad _mainRoad;
    [SerializeField] RendererPrefabsCollection _largeBuildings;
    [SerializeField] RendererPrefabsCollection _mediumBuildings;
    [SerializeField] RendererPrefabsCollection _smallBuildings;
    [SerializeField] PositionsService _positionsService;
    public override void InstallBindings()
    {
        Container.Bind<LargeBuildingCollection>().FromInstance(_largeBuildings as LargeBuildingCollection).AsSingle();
        Container.Bind<MediumBuildingCollection>().FromInstance(_mediumBuildings as MediumBuildingCollection).AsSingle();
        Container.Bind<SmallBuildingCollection>().FromInstance(_smallBuildings as SmallBuildingCollection).AsSingle();
        Container.Bind<MainRoad>().FromInstance(_mainRoad).AsSingle();
        Container.Bind<PositionsService>().FromInstance(_positionsService).AsSingle();
    }
}