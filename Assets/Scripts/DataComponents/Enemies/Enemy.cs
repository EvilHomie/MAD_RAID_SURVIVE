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

    public Bounds CombinedBounds { get => _combinedBounds; set => _combinedBounds = value; }
    public AbstractVehiclePart[] AllVehicleParts { get => _allVehicleParts; set => _allVehicleParts = value; }

    public float _lastZPos;
    public bool _isDead;
    public FollowerEntity IAstarAI => _IAstarAI;
    public Rigidbody Rb => _rb;
    public NavmeshCut NavmeshCut => _navmeshCut;
    public AbstractMoverPart[] MoveParts => _moveParts;
    public VehiclePartType MoverPartsType => _moverPartsType;
    public bool IsDead => _isDead;


    EventBus _eventBus;
    FollowerEntity _IAstarAI;

    [Inject]
    public void Construct(EventBus eventBus, EnemyHpService enemyHpService)
    {
        _eventBus = eventBus;
        _IAstarAI = transform.root.GetComponent<FollowerEntity>();
        foreach (var part in _allVehicleParts)
        {
            part.Init(_powerMod, enemyHpService);
        }
    }


    //тестовая часть. Удалить по завершению логики
    [SerializeField] bool testDie;
    private void Update()
    {
        if (testDie && !_isDead)
        {
            _isDead = true;
            _eventBus.OnEnemyDie?.Invoke(this);
        }
    }
    // конец тестовой части
}
