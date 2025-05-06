using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class AbstractVehiclePart : MonoBehaviour, IDamageable
{
    [SerializeField] VehiclePartType _partType;
    [SerializeField] float _currentHPValue;
    [SerializeField] Renderer[] _partsReactingOnHit;

    float _HPValueMax;
    int _emissionValuePropertyID;
    float _lastHitTime;
    Color _criticalHPColor;

    UniTask _emissionTask;

    Config _config;
    private void Awake()
    {
        _emissionValuePropertyID = Shader.PropertyToID("_EmissionValue");
    }
    public void Init(float hpMod, EnemyHpService enemyHpService, Config config)
    {
        _config = config;
        _currentHPValue = enemyHpService.GetHPValueByType(_partType) * hpMod;
        _HPValueMax = _currentHPValue;
        _criticalHPColor = config.CriticalHPColor;
    }
    public void OnDamaged(float hitValue, float showDuration)
    {
        _currentHPValue -= hitValue;

        _lastHitTime = Time.time;
        if (_emissionTask.Status.IsCompleted())
        {
            _emissionTask = OnDamagedEmissionTask(showDuration);
        }
    }


    async UniTask OnDamagedEmissionTask(float duration)
    {        
        float timer = Time.time;
        if (duration == 0)
        {
            SetDamageEmission();
        }
        else
        {
            while (timer <= _lastHitTime + duration && !destroyCancellationToken.IsCancellationRequested)
            {
                timer += Time.deltaTime;
                SetDamageEmission();
                await UniTask.Yield();
            }
            foreach (var renderer in _partsReactingOnHit)
            {
                renderer.material.SetColor(_emissionValuePropertyID, Color.black);

            }
        }
    }

    void SetDamageEmission()
    {
        foreach (var renderer in _partsReactingOnHit)
        {
            float damageInterpolation = Mathf.InverseLerp(_HPValueMax, 0 , _currentHPValue);
            Color color = _criticalHPColor * _config.CriticalHPCurve.Evaluate(damageInterpolation);
            renderer.material.SetColor(_emissionValuePropertyID, color);
        }
    }
}
