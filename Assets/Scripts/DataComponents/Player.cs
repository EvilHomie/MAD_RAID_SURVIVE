using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    [SerializeField] Weapon _weapon;

    EventBus _eventBus;

    [Inject]
    public void Construct( EventBus eventBus)
    {
        _eventBus = eventBus;
    }

    private void Start()
    {
        _eventBus.OnPlayerChangeWeapon?.Invoke(_weapon);
    }
}
