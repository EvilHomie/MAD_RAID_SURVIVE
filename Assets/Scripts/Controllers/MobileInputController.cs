using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Zenject;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class MobileInputController : AbstractInputController
{
    Finger _finger;
    Vector2 _lastPos;

    [Inject]
    public void Construct()
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
            var delta = movedFinger.currentTouch.screenPosition - _lastPos;
            OnMoveCursorDelta?.Invoke(delta * _sensitivity);
            _lastPos = movedFinger.currentTouch.screenPosition;
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
            _lastPos = _finger.screenPosition;
            OnPressAttackBtn?.Invoke();
        }
    }

    protected override void DisableController()
    {
        EnhancedTouchSupport.Disable();
    }

    protected override void EnableController()
    {
        EnhancedTouchSupport.Enable();
    }
}
