using UnityEngine;
using Zenject;

public class PlayerAIMService : AbstractInRaidService
{
    [SerializeField] LayerMask layerMask;
    AbstractInputController _controller;
    AbstractWeapon _playerCurrentWeapon;

    float _vertRotation = 0;
    float _horRotation = 0;

    [Inject]
    public void Construct(AbstractInputController abstractInputController)
    {
        _controller = abstractInputController;
        _eventBus.OnPlayerChangeWeapon += OnChangeWeapon;
        _controller.OnMoveCursorDelta += OnMoveCursor;
        
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

    protected override void OnStartRaid()
    {
        _vertRotation = 0;
        _horRotation = 0;
        Cursor.lockState = CursorLockMode.Locked;
        _gameFlowService.CustomUpdate += AimWeapon;
    }

    protected override void OnStopRaid()
    {
        _gameFlowService.CustomUpdate -= AimWeapon;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnChangeWeapon(AbstractWeapon weapon)
    {
        _playerCurrentWeapon = weapon;
    }   

    void OnMoveCursor(Vector2 delta)
    {
        _vertRotation -= delta.y;
        _vertRotation = Mathf.Clamp(_vertRotation, -_config.VertMaxRotationAngle, _config.VertMaxRotationAngle);
        _horRotation += delta.x;
        _horRotation = Mathf.Clamp(_horRotation, -_config.HorMaxRotationAngle, _config.HorMaxRotationAngle);

        Camera.main.transform.eulerAngles = new Vector3(_vertRotation, _horRotation, 0);
        
    }

    Vector3 GetAimPos()
    {
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit firePointhitInfo, 1000000, layerMask);
        return firePointhitInfo.point;
    }
    private void AimWeapon()
    {
        _playerCurrentWeapon.Aim(GetAimPos());
    }
}
