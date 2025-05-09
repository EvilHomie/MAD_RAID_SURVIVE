using UnityEngine;

public class Caterpillar : AbstractMoverPart
{
    [SerializeField] Renderer _tape;
    [SerializeField] Transform[] _additionalRotationParts;

    int _mainTextureOffsetValuePropertyID;
    Vector2 _lastOffset;

    public override void Init(float hpMod, EnemyHpService enemyHpService)
    {
        _mainTextureOffsetValuePropertyID = Shader.PropertyToID("_TextureOffset");
        _partType = VehiclePartType.Caterpillar;
        base.Init(hpMod, enemyHpService);
    }

    public override void MoveForwardAnimationTick(float speed, float adModValue = 0)
    {
        _lastOffset.x += speed;
        _tape.material.SetVector(_mainTextureOffsetValuePropertyID, _lastOffset);
        foreach (Transform t in _additionalRotationParts)
        {
            t.Rotate(Vector3.right, speed * adModValue, Space.Self);
        }
    }

    //public override void SidewaysTurnAnimationTick(float tickValue, float rotateSpeed)
    //{
    //    if (!_withSidewaysTurnAnimation) return;
    //    Vector3 rotation = transform.localRotation.eulerAngles;
    //    rotation.y = tickValue;
    //    transform.localRotation = Quaternion.Euler(rotation);
    //}

    public override void UpdateSidewaysTurnAngle(float newAngle)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateRotation(float rotateSpeed)
    {
        throw new System.NotImplementedException();
    }
}
