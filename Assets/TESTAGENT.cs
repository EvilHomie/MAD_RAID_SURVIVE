using UnityEngine;
using UnityEngine.AI;

public class TESTAGENT : MonoBehaviour
{
    [SerializeField] Vector3 targetPos;

    private NavMeshPath path;
    private float elapsed = 0.0f;


    private void Start()
    {
        //NavMeshAgent agent = GetComponent<NavMeshAgent>();
        //agent.SetDestination(targetPos);

        path = new NavMeshPath();
        elapsed = 0.0f;

    }

    private void Update()
    {
        // Update the way to the goal every second.
        elapsed += Time.deltaTime;
        //Debug.Log(elapsed);
        if (elapsed > 1.0f)
        {
            elapsed -= 1.0f;
            
            Debug.Log(NavMesh.CalculatePath(transform.position, targetPos, NavMesh.AllAreas, path));
        }
        
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
            Debug.Log($" {path.corners[i]}, {path.corners[i + 1]}");
        }
    }
}
