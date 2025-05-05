using System;
using UnityEngine;
using Zenject;


public abstract class AbstractInputController
{
    public Action<Vector2> OnMoveCursorDelta;
    public Action OnPressAttackBtn;
    public Action OnReleaseAttackBtn;

    protected float _sensitivity;
    protected Config _config;
    EventBus _eventBus;

    bool _enabled = false;

    [Inject]
    public void Construct(Config config, EventBus eventBus)
    {
        _enabled = true;
        _config = config;
        _eventBus = eventBus;
        _eventBus.OnPauseFight += DisableController;
        _eventBus.OnResumeFight += EnableController;
    }

    protected virtual void DisableController()
    {
        if (!_enabled) return;
    }
    protected virtual void EnableController()
    {
        if (_enabled) return;
    }
}

