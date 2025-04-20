using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class TESTRAIDMANAGER : MonoBehaviour
{
    EventBus _eventBus;

    [Inject]
    public void Construct(EventBus eventBus)
    {
        _eventBus = eventBus;
    }

    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            _eventBus.OnStartRaid?.Invoke();
        }

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            _eventBus.OnStopRaid?.Invoke();
        }
    }
}
