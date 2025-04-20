using UnityEngine;

public class GunPoint : MonoBehaviour
{
    Vector3 _deffPos;
    public Vector3 DeffPos => _deffPos;
    void OnEnable()
    {
        _deffPos = transform.localPosition;
    }
}
