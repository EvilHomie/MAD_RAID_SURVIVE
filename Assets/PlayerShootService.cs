using UnityEngine;
using Zenject;

public class PlayerShootService : MonoBehaviour
{
    AbstractInputController _controller;
    EventBus _eventBus;
    AbstractWeapon _playerLastWeapon;

    [Inject]
    public void Construct(AbstractInputController abstractInputController, EventBus eventBus)
    {
        _eventBus = eventBus;
        _controller = abstractInputController;
        _controller.OnPressAttackBtn += OnStartShoot;
        _controller.OnReleaseAttackBtn += OnStopShoot;
        _eventBus.OnPlayerChangeWeapon += OnChangeWeapon;
    }

    private void OnChangeWeapon(AbstractWeapon weapon)
    {
        _playerLastWeapon = weapon;
    }

    void OnStartShoot()
    {
        _playerLastWeapon.StartShoot();
    }

    void OnStopShoot()
    {
        _playerLastWeapon.StopShoot();
    }
}
