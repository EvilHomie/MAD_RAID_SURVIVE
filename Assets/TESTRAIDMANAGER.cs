using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class TESTRAIDMANAGER : MonoBehaviour
{
    EventBus _eventBus;

    bool _InRaid = false;

    [Inject]
    public void Construct(EventBus eventBus)
    {
        _eventBus = eventBus;
    }


    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            _InRaid = !_InRaid;

            if (_InRaid)
            {
                _eventBus.OnStartRaid?.Invoke();
            }
            else
            {
                _eventBus.OnStopRaid?.Invoke();
            }
        }
    }
}
