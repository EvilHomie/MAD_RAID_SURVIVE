using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;

public abstract class AbstractVehiclePart : MonoBehaviour, IDamageable, IDetachable
{
    [SerializeField] VehiclePartType _partType;
    [SerializeField] Renderer[] _associatedRenderers;
    Material[] _associatedMaterials;

    float _currentHPValue;
    float _maxHpValue;
    float _hitEmissionTimer;
    bool _colorInited;

    public float MaxHPValue => _maxHpValue;

    public float CurrentHPValue => _currentHPValue;
    public Material[] AssociatedMaterials => _associatedMaterials;

    public float HitEmissionTimer { get => _hitEmissionTimer; set => _hitEmissionTimer = value; }
    public bool ColorInited { get => _colorInited; set => _colorInited = value; }

    public void Init(float hpMod, EnemyHpService enemyHpService)
    {

        _associatedMaterials = _associatedRenderers.Select(renderer => renderer.material).ToArray();
        _currentHPValue = enemyHpService.GetHPValueByType(_partType) * hpMod;
        _maxHpValue = _currentHPValue;
    }


    public void OnDamaged(float hitValue)
    {
        _currentHPValue -= hitValue;
        //Debug.Log(_currentHPValue);
    }
}
