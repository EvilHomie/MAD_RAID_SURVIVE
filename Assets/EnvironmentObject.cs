using UnityEngine;

public class EnvironmentObject : MonoBehaviour
{
    public Renderer ObjectRenderer => _objectRenderer;
    [SerializeField] Renderer _objectRenderer;
}
