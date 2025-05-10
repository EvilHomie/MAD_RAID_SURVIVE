using System;
using UnityEngine;

public class MainRoad : MonoBehaviour, IHitable
{
    public Action<Collision> collisionEvent;
    public void OnHit()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisionEvent?.Invoke(collision);
    }
}
