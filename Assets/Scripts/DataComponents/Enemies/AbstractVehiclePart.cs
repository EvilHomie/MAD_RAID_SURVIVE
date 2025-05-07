using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;

public abstract class AbstractVehiclePart : MonoBehaviour, IDamageable
{
    [SerializeField] Renderer[] _associatedRenderers;

    public float MaxHPValue => _maxHpValue;
    public float CurrentHPValue => _currentHPValue;
    public Material[] AssociatedMaterials => _associatedMaterials;
    public float HitEmissionTimer { get => _hitEmissionTimer; set => _hitEmissionTimer = value; }
    public bool EmissionInited { get => _emissionInited; set => _emissionInited = value; }

    protected VehiclePartType _partType;

    Material[] _associatedMaterials;
    float _currentHPValue;
    float _maxHpValue;
    float _hitEmissionTimer;
    bool _emissionInited;

    public virtual void Init(float hpMod, EnemyHpService enemyHpService)
    {
        _associatedMaterials = _associatedRenderers.Select(renderer => renderer.material).ToArray();
        _currentHPValue = enemyHpService.GetHPValueByType(_partType) * hpMod;
        _maxHpValue = _currentHPValue;
    }

    public void OnDamaged(float hitValue)
    {
        _currentHPValue -= hitValue;
    }
}
