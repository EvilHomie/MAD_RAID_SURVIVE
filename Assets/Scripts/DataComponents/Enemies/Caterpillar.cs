using UnityEngine;

public class Caterpillar : AbstractMoverPart
{
    [SerializeField] Renderer _tape;
    [SerializeField] Transform[] _additionalRotationParts;
    Vector2 _textureOffset;

    public Renderer Tape => _tape;
    public Transform[] AdditionalRotationParts => _additionalRotationParts;

    public Vector2 TextureOffset { get => _textureOffset; set => _textureOffset = value; }

    public override void Init(float hpMod, EnemyHpService enemyHpService)
    {
        _partType = VehiclePartType.Caterpillar;
        base.Init(hpMod, enemyHpService);
    }
}
