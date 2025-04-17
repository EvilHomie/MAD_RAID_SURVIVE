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

    private void Start()
    {
        _eventBus.OnStartRaid?.Invoke();
    }
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Time.timeScale = 1;
        }
    }
}
