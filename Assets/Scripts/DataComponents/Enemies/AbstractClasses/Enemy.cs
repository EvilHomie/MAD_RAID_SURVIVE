using Pathfinding;
using UnityEngine;
using Zenject;

public abstract class Enemy : MonoBehaviour, IRendererBounds
{
    [SerializeField] Bounds _combinedBounds;
    [SerializeField] Rigidbody _rb;
    [SerializeField] NavmeshCut _navmeshCut;
    [SerializeField] AbstractMoverPart[] _moveParts;
    [SerializeField] VehiclePartType _moverPartsType;
    [SerializeField] AbstractVehiclePart[] _allVehicleParts;
    [SerializeField] float _powerMod;
    [SerializeField] VehicleBody _vehicleBody;

    EventBus _eventBus;
    FollowerEntity _IAstarAI;
    float _lastZPos;
    bool _isDead;


    public Bounds CombinedBounds { get => _combinedBounds; set => _combinedBounds = value; }
    public AbstractVehiclePart[] AllVehicleParts { get => _allVehicleParts; set => _allVehicleParts = value; }
    public VehicleBody VehicleBody { get => _vehicleBody; set => _vehicleBody = value; }
    public FollowerEntity IAstarAI => _IAstarAI;
    public Rigidbody Rigidbody => _rb;
    public NavmeshCut NavmeshCut => _navmeshCut;
    public AbstractMoverPart[] MoveParts => _moveParts;
    public VehiclePartType MoverPartsType => _moverPartsType;
    public bool IsDead
    {
        get => _isDead;
        set
        {
            _isDead = value;
            if (_isDead) _eventBus.OnEnemyDie?.Invoke(this);
        }
    }
    public float LastZPos { get => _lastZPos; set => _lastZPos = value; }


    

    [Inject]
    public void Construct(EventBus eventBus, EnemyHpService enemyHpService)
    {
        _eventBus = eventBus;
        _IAstarAI = transform.root.GetComponent<FollowerEntity>();
        VehicleBody.Init(_powerMod, enemyHpService, this);
        foreach (var part in _allVehicleParts)
        {
            part.Init(_powerMod, enemyHpService, this);
        }
    }


    //тестовая часть. Удалить по завершению логики
    //[SerializeField] bool testDie;
    //private void Update()
    //{
    //    if (testDie && !_isDead)
    //    {
    //        _isDead = true;
    //        _eventBus.OnEnemyDie?.Invoke(this);
    //    }
    //}
    // конец тестовой части
}
