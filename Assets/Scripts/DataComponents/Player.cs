using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    [SerializeField] AbstractWeapon _weapon;

    EventBus _eventBus;

    AbstractWeapon _test;
    Config _config;

    [Inject]
    public void Construct( EventBus eventBus, Config config)
    {
        _eventBus = eventBus;
        _config = config;
        
    }

    private void Start()
    {
        _eventBus.OnPlayerChangeWeapon?.Invoke(_weapon);
    }

    private void Update()
    {
        if (_weapon != _test)
        {
            _test = _weapon;
            _weapon.Init(_config, destroyCancellationToken);
            _eventBus.OnPlayerChangeWeapon?.Invoke(_weapon);
        }
    }
}