using UnityEngine;

public class PrefabsCollection : ScriptableObject
{
    [SerializeField] Transform[] _prefabs;
    public Transform[] Prefabs => _prefabs;
}
