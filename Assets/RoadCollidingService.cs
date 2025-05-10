using UnityEngine;
using Zenject;

public class RoadCollidingService : AbstractInRaidService
{
    DetachService _detachService;
    MainRoad _mainRoad;
    [Inject]
    public void Construct(MainRoad mainRoad, DetachService detachService)
    {
        _detachService = detachService;
        _mainRoad = mainRoad;
    }
    protected override void OnStartRaid()
    {
        _mainRoad.collisionEvent += OnCollisionEnterAction;
    }
    protected override void OnStopRaid()
    {
        _mainRoad.collisionEvent -= OnCollisionEnterAction;
    }

    private void OnCollisionEnterAction(Collision collision)
    {
        if (collision.collider.gameObject.TryGetComponent<IDamageable>(out var damagedPart))
        {
            if (damagedPart.VehiclePartType == VehiclePartType.Wheel || damagedPart.VehiclePartType == VehiclePartType.Caterpillar)
            {
                ActionOnMovePartCollided(damagedPart);
            }
            else if (damagedPart.VehiclePartType == VehiclePartType.Body)
            {
                ActionOnBodyCollided(damagedPart);
            }
            else
            {
                ActionForOtherCollision(damagedPart);
            }
        }
    }

    void ActionOnMovePartCollided(IDamageable damagedPart)
    {
        if (!damagedPart.AssociatedEnemy.IsDead) return;
        DestroyPart(damagedPart);
    }

    void ActionOnBodyCollided(IDamageable damagedPart)
    {
        damagedPart.CurrentHPValue = 0;
        damagedPart.AssociatedEnemy.IsDead = true;
    }

    void ActionForOtherCollision(IDamageable damagedPart)
    {
        DestroyPart(damagedPart);
    }

    void DestroyPart(IDamageable damagedPart)
    {
        if(damagedPart.CurrentHPValue <= 0) return;
        damagedPart.CurrentHPValue = 0;
        if (damagedPart.GameObject.TryGetComponent<IDetachable>(out var detachedPart))
        {
            _detachService.DetachPart(detachedPart);
        }
    }
}
