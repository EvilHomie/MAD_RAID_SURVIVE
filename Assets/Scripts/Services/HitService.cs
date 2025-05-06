using System;
using UnityEngine;

public class HitService : AbstractInRaidService
{
    protected override void OnStartRaid()
    {
        _eventBus.OnPlayerHitsSomething += OnPlayerHitsSomething;
    }
    protected override void OnStopRaid()
    {
        _eventBus.OnPlayerHitsSomething -= OnPlayerHitsSomething;
    }

    private void OnPlayerHitsSomething(GameObject hitedObj, Vector3 hitPos, float damage)
    {
        if (hitedObj.TryGetComponent<IDamageable>(out var damageable)) 
        {
            damageable.OnDamaged(damage, _config.ShowHitDuration);
        }

    }



}
