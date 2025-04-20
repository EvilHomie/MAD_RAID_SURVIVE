using UnityEngine;
using Zenject;

public class PlayerShootService : MonoBehaviour
{
    AbstractInputController _controller;
    EventBus _eventBus;
    Weapon _playerLastWeapon;

    [Inject]
    public void Construct(AbstractInputController abstractInputController, EventBus eventBus)
    {
        _eventBus = eventBus;
        _controller = abstractInputController;
        _controller.OnPressAttackBtn += OnStartShoot;
        _controller.OnReleaseAttackBtn += OnStopShoot;
        _eventBus.OnPlayerChangeWeapon += OnChangeWeapon;
    }

    private void OnChangeWeapon(Weapon weapon)
    {
        _playerLastWeapon = weapon;
    }

    void OnStartShoot()
    {
        _playerLastWeapon.OnStartShoot();
    }

    void OnStopShoot()
    {
        _playerLastWeapon.OnStopShoot();
    }
}
