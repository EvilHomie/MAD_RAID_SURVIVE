using UnityEngine;

public class EnvironmentPrefabsCollection : ScriptableObject
{
    [SerializeField] EnvironmentObject[] _prefabs;
    public EnvironmentObject[] Prefabs => _prefabs;
}
