using UnityEngine;

[CreateAssetMenu(fileName = "VehiclePartsHP", menuName = "Scriptable Objects/VehiclePartsHP")]
public class VehiclePartsHP : ScriptableObject
{
    public float WheelHP => _wheelHP;
    public float CaterpillarHP => _caterpillarHP;
    public float ArmoredWheelHP => _armoredWheelHP;
    public float ExplosionPartHP => _explosionPartHP;
    public float ProtectionHP => _protectionHP;
    public float OtherAttachmentHP => _otherAttachmentHP;
    public float WeaponHP => _weaponHP;


    [SerializeField] float _wheelHP;
    [SerializeField] float _caterpillarHP;
    [SerializeField] float _armoredWheelHP;
    [SerializeField] float _explosionPartHP;
    [SerializeField] float _protectionHP;
    [SerializeField] float _otherAttachmentHP;
    [SerializeField] float _weaponHP;
}
