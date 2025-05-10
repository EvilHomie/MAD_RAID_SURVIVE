using UnityEngine;
using Zenject;

public class HitService : AbstractInRaidService
{
    DetachService _detachService;
    HitVisualService _hitVisualService;

    [Inject]
    public void Construct(DetachService detachService, HitVisualService hitVisualService)
    {
        _detachService = detachService;
        _hitVisualService = hitVisualService;
    }
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
        if (hitedObj.TryGetComponent<IDamageable>(out var damagedPart))
        {
            if (damagedPart.CurrentHPValue <= 0) return;

            damagedPart.CurrentHPValue -= damage;

            if (damagedPart.CurrentHPValue > 0)
            {
                _hitVisualService.OnPlayerDamageSomething(damagedPart);
            }
            else
            {
                AditioanalActionOnDestroyPart(damagedPart);
                if (hitedObj.TryGetComponent<IDetachable>(out var detachedPart))
                {
                    Detach(detachedPart);
                }
            }
        }
    }
    void Detach(IDetachable detachedPart)
    {
        _detachService.DetachPart(detachedPart);
    }


    void AditioanalActionOnDestroyPart(IDamageable damagedPart)
    {
        switch (damagedPart.VehiclePartType)
        {
            case VehiclePartType.Wheel:
                ChangeCenterOfMass(damagedPart);
                break;

            case VehiclePartType.Caterpillar:
                ChangeCenterOfMass(damagedPart);
                break;

            case VehiclePartType.Body:
                damagedPart.AssociatedEnemy.IsDead = true;
                break;
        }
    }

    void ChangeCenterOfMass(IDamageable damagedPart)
    {
        var rootRB = damagedPart.AssociatedEnemy.Rigidbody;

        Vector3 newCenterOfMass = Vector3.forward * _config.LoseMovePartChangeCenterOfMassMod;
        if (damagedPart.GameObject.transform.localPosition.z <= 0)
        {
            newCenterOfMass = -newCenterOfMass;
        }
        rootRB.centerOfMass = newCenterOfMass;
    }
}
