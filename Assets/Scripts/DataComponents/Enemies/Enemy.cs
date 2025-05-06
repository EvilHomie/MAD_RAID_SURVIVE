using Pathfinding;
using UnityEngine;
using Zenject;

public abstract class Enemy : MonoBehaviour, IRendererBounds
{
    public Bounds CombinedBounds
    {
        get => _combinedBounds;
        set => _combinedBounds = value;
    }
    public AbstractVehiclePart[] AllVehicleParts
    {
        get => _allVehicleParts;
        set => _allVehicleParts = value;
    }

    public float _lastZPos;



    public bool isDead;
    public FollowerEntity IAstarAI => _IAstarAI;
    public Rigidbody Rb => _rb;
    public NavmeshCut NavmeshCut => _navmeshCut;
    public AbstractMoverPart[] MoveParts => _moveParts;
    public VehiclePartType MoverPartsType => _moverPartsType;

    [SerializeField] Bounds _combinedBounds;
    [SerializeField] Rigidbody _rb;
    [SerializeField] NavmeshCut _navmeshCut;
    [SerializeField] AbstractMoverPart[] _moveParts;
    [SerializeField] VehiclePartType _moverPartsType;
    [SerializeField] AbstractVehiclePart[] _allVehicleParts;

    [SerializeField] float _powerMod;
    EventBus _eventBus;
    FollowerEntity _IAstarAI;

    [Inject]
    public void Construct(EventBus eventBus, EnemyHpService enemyHpService, Config config)
    {
        _eventBus = eventBus;
        _IAstarAI = transform.root.GetComponent<FollowerEntity>();
        foreach (var part in _allVehicleParts)
        {
            part.Init(_powerMod, enemyHpService, config);
        }
    }


    [SerializeField] bool testDie;

    private void Update()
    {
        if (testDie && !isDead)
        {
            isDead = true;
            _eventBus.OnEnemyDie?.Invoke(this);
        }
    }
}
