using UnityEngine;

public class PrefabsCollection : ScriptableObject
{
    [SerializeField] Renderer[] _prefabs;
    public Renderer[] Prefabs => _prefabs;
}
