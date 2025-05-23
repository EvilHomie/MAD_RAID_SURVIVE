using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    [SerializeField] AbstractWeapon _newWeapon;

    EventBus _eventBus;

    AbstractWeapon _currentWeapon;
    Config _config;

    [Inject]
    public void Construct(EventBus eventBus, Config config)
    {
        _eventBus = eventBus;
        _config = config;
    }
    private void Start()
    {
        OnChangeWeapon();
    }

    private void Update()
    {
        if (_currentWeapon != _newWeapon)
        {
            OnChangeWeapon();
        }
    }

    void OnChangeWeapon()
    {
        _currentWeapon = _newWeapon;
        _currentWeapon.Init(_config, destroyCancellationToken, OnHitGameObject);
        _eventBus.OnPlayerChangeWeapon?.Invoke(_currentWeapon);
    }

    void OnHitGameObject(GameObject hitedObject, Vector3 hitPos, float damage)
    {
        _eventBus.OnPlayerHitsSomething(hitedObject, hitPos, damage);
        //Debug.Log(hitedObject.name);
        //Debug.Log(hitPos);
        //Debug.Log(damage);
    }
}