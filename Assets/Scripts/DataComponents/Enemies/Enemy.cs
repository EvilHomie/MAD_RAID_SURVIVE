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

    public bool isDead;

    public FollowerEntity IAstarAI => _IAstarAI;
    public Rigidbody Rb => _rb;
    public NavmeshCut NavmeshCut => _navmeshCut;

    [SerializeField] Bounds _combinedBounds;
    [SerializeField] Rigidbody _rb;
    [SerializeField] NavmeshCut _navmeshCut;


    EventBus _eventBus;
    FollowerEntity _IAstarAI;

    [Inject]
    public void Construct(EventBus eventBus)
    {
        _eventBus = eventBus;
        _IAstarAI = transform.root.GetComponent<FollowerEntity>();
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
