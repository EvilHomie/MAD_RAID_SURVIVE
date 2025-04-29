using UnityEngine;
using Zenject;

public class PlayerShootService : MonoBehaviour
{
    AbstractInputController _controller;
    EventBus _eventBus;
    AbstractWeapon _playerCurrentWeapon;

    [Inject]
    public void Construct(AbstractInputController abstractInputController, EventBus eventBus)
    {
        _eventBus = eventBus;
        _controller = abstractInputController;
        _eventBus.OnPlayerChangeWeapon += OnChangeWeapon;
        _controller.OnPressAttackBtn += OnStartShoot;
        _controller.OnReleaseAttackBtn += OnStopShoot;
    }

    private void OnChangeWeapon(AbstractWeapon weapon)
    {
        _playerCurrentWeapon = weapon;
    }

    void OnStartShoot()
    {
        _playerCurrentWeapon.StartShooting();
    }

    void OnStopShoot()
    {
        _playerCurrentWeapon.StopShooting();
    }
}
