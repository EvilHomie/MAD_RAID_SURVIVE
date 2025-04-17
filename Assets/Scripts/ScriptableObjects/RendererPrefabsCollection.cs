using UnityEngine;

public class RendererPrefabsCollection : ScriptableObject
{
    [SerializeField] Renderer[] _prefabs;
    public Renderer[] Prefabs => _prefabs;
}
