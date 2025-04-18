using UnityEngine.InputSystem.EnhancedTouch;

public class MobileInputController : AbstractInputController
{
    Finger _finger;
    public MobileInputController()
    {
        _sensitivity = _config.MobileMouseSensitivity;
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += HandleFingerDown;
        Touch.onFingerUp += HandleFingerLose;
        Touch.onFingerMove += HandleFingerMove;
    }

    void HandleFingerMove(Finger movedFinger)
    {
        if (movedFinger == _finger)
        {
            OnMoveCursorDelta?.Invoke(movedFinger.currentTouch.delta * _sensitivity);
        }
    }

    void HandleFingerLose(Finger lostFinger)
    {
        if (lostFinger == _finger)
        {
            _finger = null;
            OnReleaseAttackBtn?.Invoke();
        }
    }

    void HandleFingerDown(Finger touchedTinger)
    {
        if (_finger == null)
        {
            _finger = touchedTinger;
            OnPressAttackBtn?.Invoke();
        }
    }

    protected override void DisableController()
    {
        EnhancedTouchSupport.Disable();
    }

    protected override void ReturnController()
    {
        EnhancedTouchSupport.Enable();
    }
}
