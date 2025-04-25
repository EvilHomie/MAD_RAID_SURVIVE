using UnityEngine;
using Zenject;

public class PlayerAIMService : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    AbstractInputController _controller;
    CannonPivot _cannonPivot;
    EventBus _eventBus;
    Config _config;

    float _vertRotation = 0;
    float _horRotation = 0;
    Vector3 _rotation;

    [Inject]
    public void Construct(AbstractInputController abstractInputController, CannonPivot cannonPivot, EventBus eventBus, Config config)
    {
        _config = config;
        _cannonPivot = cannonPivot;
        _eventBus = eventBus;
        _controller = abstractInputController;
        _controller.OnMoveCursorDelta += OnMoveCursor;
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
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

    void OnMoveCursor(Vector2 delta)
    {
        _vertRotation -= delta.y;
        _vertRotation = Mathf.Clamp(_vertRotation, -_config.VertMaxRotationAngle, _config.VertMaxRotationAngle);
        _horRotation += delta.x;
        _horRotation = Mathf.Clamp(_horRotation, -_config.HorMaxRotationAngle, _config.HorMaxRotationAngle);
        _rotation = new Vector3(_vertRotation, _horRotation, 0);



        _cannonPivot.transform.LookAt(GetAimPos());
        Camera.main.transform.eulerAngles = _rotation;
    }

    Vector3 GetAimPos()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit firePointhitInfo, 1000000, layerMask))
        {
            Debug.Log(firePointhitInfo.transform.name);

        }
        return firePointhitInfo.point;
        //Vector3 markerDir = Vector3.Normalize(firePointhitInfo.point - Camera.main.transform.position);
        //Vector3 pos = Camera.main.transform.position + markerDir * 200;
        //_weaponsByIndex[_selectedWeaponIndex].TargetMarker.transform.position = pos;

    }


}
