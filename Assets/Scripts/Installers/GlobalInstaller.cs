using UnityEngine;
using YG;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] Config _config;
    [SerializeField] GameFlowService _gameFlowService;
    [SerializeField] EnemiesCollection[] _enemiesCollections;
    

    public override void InstallBindings()
    {
        Container.Bind<Config>().FromInstance(_config).AsSingle();
        Container.Bind<EventBus>().FromInstance(new()).AsSingle();
        Container.Bind<GameFlowService>().FromInstance(_gameFlowService).AsSingle();
        Container.Bind<EnemiesCollection[]>().FromInstance(_enemiesCollections).AsSingle();
        

        if (YG2.envir.isDesktop)
        {
            Container.Bind<AbstractInputController>().To<PCInputController>().FromNew().AsSingle();
        }
        else
        {
            Container.Bind<AbstractInputController>().To<MobileInputController>().FromNew().AsSingle();
        }
    }
}


