using UnityEngine;
using UnityEngine.Rendering;

public class Wheel : AbstractMoverPart
{
    public override void MoveRotateAnimationTick(float tickValue, float adModValue = 0)
    {
        transform.Rotate(Vector3.right, tickValue, Space.Self);
    }

    public override void SidewaysTurnAnimationTick(float rotateValue, float rotateSpeed)
    {
        if (!_withSidewaysTurnAnimation) return;
        Vector3 rotation = transform.localRotation.eulerAngles;
        rotation.y = rotateValue;

        //transform.Rotatte(Vector3.up, )

        //transform.localRotation = Quaternion.Euler(rotation);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(rotation), rotateSpeed);
    }
}
