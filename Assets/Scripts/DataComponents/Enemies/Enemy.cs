using Pathfinding;
using UnityEngine;
using Zenject;

public abstract class Enemy : MonoBehaviour
{
    public Bounds CombinedBounds => _combinedBounds;
    public IAstarAI IAstarAI => _IAstarAI;

    IAstarAI _IAstarAI;
    NavmeshCut _navmeshCut;
    EventBus _eventBus;
    [SerializeField] Bounds _combinedBounds;

    [Inject]
    public void Construct(EventBus eventBus)
    {
        _eventBus = eventBus;
        _IAstarAI = GetComponent<IAstarAI>();
        _navmeshCut = GetComponent<NavmeshCut>();
    }

    public void UpdateBounds(Bounds bounds)
    {
        _combinedBounds = bounds;
    }
}
