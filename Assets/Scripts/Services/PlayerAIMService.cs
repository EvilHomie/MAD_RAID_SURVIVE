using UnityEngine;
using Zenject;

public class PlayerAIMService : MonoBehaviour
{
    AbstractInputController _controller;
    CannonPivot _cannonPivot;
    EventBus _eventBus;

    float _vertRotation = 0;
    float _horRotation = 0;

    [Inject]
    public void Construct(AbstractInputController abstractInputController, CannonPivot cannonPivot, EventBus eventBus)
    {
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
        _vertRotation = Mathf.Clamp(_vertRotation, -25f, 25f);
        _horRotation += rotationDelta.x;
        _horRotation = Mathf.Clamp(_horRotation, -75, 75f);
        Cursor.lockState = CursorLockMode.Locked;
        _cannonPivot.transform.eulerAngles = new Vector3(_vertRotation, _horRotation, 0);
    }
}
