using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{
    CullingGroup group;
    BoundingSphere[] spheres;


    [SerializeField] float _distance;


    private void Start()
    {
        group = new CullingGroup();
        spheres = new BoundingSphere[2];
        group.targetCamera = Camera.main;
        
    }
    private void Update()
    {
        spheres[0] = new BoundingSphere(Vector3.zero, _distance);
        group.SetBoundingSpheres(spheres);
        group.SetBoundingSphereCount(1);
    }

    private void OnDisable()
    {
        group.Dispose();
        group = null;
    }
}
