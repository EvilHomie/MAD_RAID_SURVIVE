using UnityEngine;

public class VehiclePart : MonoBehaviour, IHitable
{
    [SerializeField] VehiclePartType _partType;
    [SerializeField] float _hpValue;
    [SerializeField] Renderer[] _partsReactingOnHit;

    int _emissionValuePropertyID;
    private void Awake()
    {
        _emissionValuePropertyID = Shader.PropertyToID("_EmissionValue");
    }
    public void Init(float hpMod, EnemyHpService enemyHpService)
    {
        _hpValue = enemyHpService.GetHPValueByType(_partType) * hpMod;
    }
    public void OnHit(float hitValue)
    {
       
    }
}
