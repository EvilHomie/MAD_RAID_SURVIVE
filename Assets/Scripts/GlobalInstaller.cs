using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] Config _config;
    [SerializeField] GameFlowService _gameFlowService;
    [SerializeField] EnemiesCollection[] _enemiesCollections;

    public override void InstallBindings()
    {
        Container.Bind<Config>().FromInstance(_config).AsSingle();
        Container.Bind<GameFlowService>().FromInstance(_gameFlowService).AsSingle();
        Container.Bind<EventBus>().FromInstance(new()).AsSingle();
        Container.Bind<EnemiesCollection[]>().FromInstance(_enemiesCollections).AsSingle();
    }
}


