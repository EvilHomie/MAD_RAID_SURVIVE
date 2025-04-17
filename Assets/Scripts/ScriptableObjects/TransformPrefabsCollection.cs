using UnityEngine;

public class TransformPrefabsCollection : ScriptableObject
{
    [SerializeField] Transform[] _prefabs;
    public Transform[] Prefabs => _prefabs;
}
