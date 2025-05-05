using UnityEngine;
using Zenject;

public abstract class AbstractInRaidService : MonoBehaviour
{
    protected EventBus _eventBus;
    protected Config _config;
    protected GameFlowService _gameFlowService;

    [Inject]
    public void Construct(Config config, GameFlowService gameFlowService, EventBus eventBus, PositionsService positionsService)
    {
        _config = config;
        _gameFlowService = gameFlowService;
        _eventBus = eventBus;
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
    protected abstract void OnStartRaid();
    protected abstract void OnStopRaid();
}
