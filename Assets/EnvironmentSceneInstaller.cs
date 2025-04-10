using UnityEngine;
using Zenject;

public class EnvironmentSceneInstaller : MonoInstaller
{
    [SerializeField] MainRoad _mainRoad;
    [SerializeField] PrefabsCollection _largeBuildings;
    [SerializeField] PrefabsCollection _mediumBuildings;
    [SerializeField] PrefabsCollection _smallBuildings;
    public override void InstallBindings()
    {
        Container.Bind<LargeBuildingCollection>().FromInstance(_largeBuildings as LargeBuildingCollection).AsSingle();
        Container.Bind<MediumBuildingCollection>().FromInstance(_mediumBuildings as MediumBuildingCollection).AsSingle();
        Container.Bind<SmallBuildingCollection>().FromInstance(_smallBuildings as SmallBuildingCollection).AsSingle();
        Container.Bind<MainRoad>().FromInstance(_mainRoad).AsSingle();
    }
}