using UnityEngine;
using Zenject;

public class PlayerAIMService : MonoBehaviour
{
    AbstractInputController _controller;
    CannonPivot _cannonPivot;
    EventBus _eventBus;
    Config _config;

    float _vertRotation = 0;
    float _horRotation = 0;

    [Inject]
    public void Construct(AbstractInputController abstractInputController, CannonPivot cannonPivot, EventBus eventBus, Config config)
    {
        _config = config;
        _cannonPivot = cannonPivot;
        _eventBus = eventBus;
        _controller = abstractInputController;
        _controller.OnMoveCursorDelta += RotateCannonPivot;
    }

    void OnEnable()
    {
        _eventBus.OnStartRaid += OnStartRaid;
    }
    void OnDisable()
    {
        _eventBus.OnStartRaid -= OnStartRaid;
    }

    protected virtual void OnStartRaid()
    {
        _vertRotation = 0;
        _horRotation = 0;
        _cannonPivot.transform.eulerAngles = Vector3.zero;
    }

    void RotateCannonPivot(Vector2 rotationDelta)
    {
        _vertRotation -= rotationDelta.y;
        _vertRotation = Mathf.Clamp(_vertRotation, -_config.VertMaxRotationAngle, _config.VertMaxRotationAngle);
        _horRotation += rotationDelta.x;
        _horRotation = Mathf.Clamp(_horRotation, -_config.HorMaxRotationAngle, _config.HorMaxRotationAngle);
        Cursor.lockState = CursorLockMode.Locked;
        _cannonPivot.transform.eulerAngles = new Vector3(_vertRotation, _horRotation, 0);
    }
}
