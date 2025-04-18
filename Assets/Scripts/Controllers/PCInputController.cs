using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PCInputController : AbstractInputController
{
    InputSystem_Actions _inputActions;

    [Inject]
    public void Construct()
    {
        _sensitivity = _config.PCMouseSensitivity;
        _inputActions = new InputSystem_Actions();
        _inputActions.PlayerInput.Look.Enable();
        _inputActions.PlayerInput.Look.performed += TrackMousePos;
        _inputActions.PlayerInput.Attack.Enable();
        _inputActions.PlayerInput.Attack.performed += AttackBtnChangeState;
    }

    protected override void DisableController()
    {
        base.DisableController();
        _inputActions.PlayerInput.Look.Disable();
        _inputActions.PlayerInput.Attack.Disable();
    }

    protected override void ReturnController()
    {
        base.ReturnController();
        _inputActions.PlayerInput.Look.Enable();
        _inputActions.PlayerInput.Attack.Enable();
    }

    void AttackBtnChangeState(InputAction.CallbackContext context)
    {
        if (context.control.IsPressed())
        {
            OnPressAttackBtn?.Invoke();
        }
        else
        {
            OnReleaseAttackBtn?.Invoke();
        }
    }

    private void TrackMousePos(InputAction.CallbackContext context)
    {
        Vector2 delta = context.ReadValue<Vector2>();
        OnMoveCursorDelta?.Invoke(delta * _sensitivity);
    }
}
