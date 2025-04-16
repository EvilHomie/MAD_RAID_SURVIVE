using Pathfinding;
using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public Action onSpawn;
    public Action onReachedEndOfPath;
    public Action onDie;
    public Action onDestroyMonoBeh;    

    public IAstarAI IAstarAI => _IAstarAI;

    IAstarAI _IAstarAI;



    protected void Awake()
    {
        _IAstarAI = GetComponent<IAstarAI>();
    }
}
