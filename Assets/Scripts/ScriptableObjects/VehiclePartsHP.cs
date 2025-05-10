using UnityEngine;

[CreateAssetMenu(fileName = "VehiclePartsHP", menuName = "Scriptable Objects/VehiclePartsHP")]
public class VehiclePartsHP : ScriptableObject
{
    public float WheelHP => _wheelHP;
    public float CaterpillarHP => _caterpillarHP;
    public float ArmoredWheelHP => _armoredWheelHP;
    public float ExplosivePartHP => _explosivePartHP;
    public float ProtectionHP => _protectionHP;
    public float OtherAttachmenWeektHP => _otherAttachmenWeektHP;
    public float OtherAttachmentArmoredHP => _otherAttachmentArmoredHP;
    public float WeaponHP => _weaponHP;
    public float BodyHP => _bodyHP;

    [SerializeField] float _wheelHP;
    [SerializeField] float _caterpillarHP;
    [SerializeField] float _armoredWheelHP;
    [SerializeField] float _explosivePartHP;
    [SerializeField] float _protectionHP;
    [SerializeField] float _otherAttachmenWeektHP;
    [SerializeField] float _weaponHP;
    [SerializeField] float _otherAttachmentArmoredHP;
    [SerializeField] float _bodyHP;
}