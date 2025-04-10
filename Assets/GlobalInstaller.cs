using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] Config _config;
    [SerializeField] GameFlowService _gameFlowService;

    public override void InstallBindings()
    {
        Container.Bind<Config>().FromInstance(_config).AsSingle();
        Container.Bind<GameFlowService>().FromInstance(_gameFlowService).AsSingle();
        Container.Bind<EventBus>().FromInstance(new()).AsSingle();
    }
}
