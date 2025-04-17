using Pathfinding;
using System;
using System.Collections;
using Unity.Entities;
using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour
{
    public Bounds CombinedBounds => _combinedBounds;
    public IAstarAI IAstarAI => _IAstarAI;

    IAstarAI _IAstarAI;
    NavmeshCut _navmeshCut;
    EventBus _eventBus;
    [SerializeField] Bounds _combinedBounds;

    public bool isDead;
    bool tempDeadCheckState;

    [Inject]
    public void Construct(EventBus eventBus)
    {
        _eventBus = eventBus;
        _IAstarAI = GetComponent<IAstarAI>();
        _navmeshCut = GetComponent<NavmeshCut>();
    }

    //void Start()
    //{
    //    _eventBus.OnSpawnEnemy?.Invoke(this);
    //}

    //IEnumerator SimSpawn()
    //{
    //    yield return new WaitForSeconds(2);
    //    _eventBus.OnSpawnEnemy?.Invoke(this);
    //}
    private void Update()
    {
        //if (!isDead && !tempDeadCheckState)
        //{
        //    tempDeadCheckState = true;
        //    _eventBus.OnEnemyDie?.Invoke(this);
        //}
    }

    public void UpdateBounds(Bounds bounds)
    {
        _combinedBounds = bounds;
    }
}
