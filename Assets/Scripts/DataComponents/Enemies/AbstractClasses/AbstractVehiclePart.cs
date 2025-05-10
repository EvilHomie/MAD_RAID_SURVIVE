using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;

public abstract class AbstractVehiclePart : MonoBehaviour, IDamageable
{
    [SerializeField] Renderer[] _associatedRenderers;

    public float MaxHPValue => _maxHpValue;
    public float CurrentHPValue { get => _currentHPValue; set => _currentHPValue = value; }
    public Material[] AssociatedMaterials => _associatedMaterials;
    public float HitEmissionTimer { get => _hitEmissionTimer; set => _hitEmissionTimer = value; }
    public bool EmissionInited { get => _emissionInited; set => _emissionInited = value; }
    public GameObject GameObject => gameObject;
    public VehiclePartType VehiclePartType => _partType;
    protected VehiclePartType _partType;
    public Enemy AssociatedEnemy => _associatedEnemy;

    Material[] _associatedMaterials;
    float _currentHPValue;
    float _maxHpValue;
    float _hitEmissionTimer;
    bool _emissionInited;
    Enemy _associatedEnemy;

    public virtual void Init(float hpMod, EnemyHpService enemyHpService, Enemy enemy)
    {
        _associatedMaterials = _associatedRenderers.Select(renderer => renderer.material).ToArray();
        _currentHPValue = enemyHpService.GetHPValueByType(_partType) * hpMod;
        _maxHpValue = _currentHPValue;
        _associatedEnemy = enemy;
    }
}
