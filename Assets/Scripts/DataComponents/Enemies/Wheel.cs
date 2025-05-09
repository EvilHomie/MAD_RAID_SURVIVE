using UnityEngine;

public class Wheel : AbstractMoverPart
{
    Vector3 _rotationVector = Vector3.right;
    float _rootRotationValue;

    [SerializeField] Vector3 _sidewaysTargetRotation;
    [SerializeField] float maxValue;

    [SerializeField] Vector3 _currentRotation;

    float _speed;
    float _currentForwardRotationValue;
    public override void Init(float hpMod, EnemyHpService enemyHpService)
    {
        _rootRotationValue = transform.root.rotation.eulerAngles.y;
        _partType = VehiclePartType.Wheel;
        base.Init(hpMod, enemyHpService);
        _currentRotation = Vector3.up * _rootRotationValue;
    }

    public override void MoveForwardAnimationTick(float speed, float adModValue = 0)
    {
        _currentForwardRotationValue += speed;
        _speed = speed;
    }



    public override void UpdateSidewaysTurnAngle(float newAngle)
    {
        _sidewaysTargetRotation = Vector3.up * (newAngle + _rootRotationValue);
    }

    public override void UpdateRotation(float rotateSpeed)
    {
        if (!_withSidewaysTurnAnimation)
        {
            transform.Rotate(Vector3.right, _speed, Space.Self);            
        }
        else
        {
            float deltaAngle = Mathf.Abs(Mathf.DeltaAngle(transform.rotation.eulerAngles.y, _sidewaysTargetRotation.y));
            float smoothMod = Mathf.InverseLerp(0, maxValue, deltaAngle);

            _currentRotation.x = 0;
            var turnRotationValue = Quaternion.RotateTowards(Quaternion.Euler(_currentRotation), Quaternion.Euler(_sidewaysTargetRotation), rotateSpeed * smoothMod);
            _currentRotation = turnRotationValue.eulerAngles;

            _currentRotation.x = _currentForwardRotationValue;
            transform.rotation = Quaternion.Euler(_currentRotation);
        }


        

        //Debug.Log($"_targetRotation: {_sidewaysTargetRotation}  deltaAngle: {deltaAngle}  smoothMod: {smoothMod}  rotateSpeed: {rotateSpeed * smoothMod}");
    }
}
