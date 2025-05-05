using UnityEngine;
using Zenject;

public class PlayerShootService : AbstractInRaidService
{
    AbstractInputController _controller;
    AbstractWeapon _playerCurrentWeapon;

    [Inject]
    public void Construct(AbstractInputController abstractInputController)
    {
        _controller = abstractInputController;
        _eventBus.OnPlayerChangeWeapon += OnChangeWeapon;
        _controller.OnPressAttackBtn += OnStartShoot;
        _controller.OnReleaseAttackBtn += OnStopShoot;
    }

    protected override void OnStartRaid()
    {
    }

    protected override void OnStopRaid()
    {
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
