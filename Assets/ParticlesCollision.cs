using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesCollision : MonoBehaviour
{
    public Action<Vector3> _collision;

    ParticleSystem _particleSystem;
    List<ParticleCollisionEvent> collisionEvents;
    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {

        int numCollisionEvents = _particleSystem.GetCollisionEvents(other, collisionEvents);

        Rigidbody rb = other.GetComponent<Rigidbody>();
        int i = 0;

        while (i < numCollisionEvents)
        {
            Vector3 pos = collisionEvents[i].intersection;
            _collision?.Invoke(pos);
            i++;
        }
    }
}
