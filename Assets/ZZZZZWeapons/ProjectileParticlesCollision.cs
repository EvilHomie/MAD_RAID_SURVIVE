using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileParticlesCollision : MonoBehaviour
{
    public Action<GameObject, Vector3> _onCollisionWithObject;

    ParticleSystem _particleSystem;
    List<ParticleCollisionEvent> collisionEvents;
    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        collisionEvents = new();
    }

    void OnParticleCollision(GameObject other)
    {
        _particleSystem.GetCollisionEvents(other, collisionEvents);

        foreach (var colEvent in collisionEvents)
        {            
            _onCollisionWithObject?.Invoke(other, colEvent.intersection);
        }

        //int numCollisionEvents = _particleSystem.GetCollisionEvents(other, collisionEvents);
        //int i = 0;
        //while (i < numCollisionEvents)
        //{
        //    Vector3 pos = collisionEvents[i].intersection;
        //    _onCollisionWithObject?.Invoke(pos);
        //    i++;
        //}
    }
}
