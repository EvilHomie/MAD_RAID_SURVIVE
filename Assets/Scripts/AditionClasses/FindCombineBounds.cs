using UnityEngine;

[ExecuteInEditMode]
public class FindCombineBounds : MonoBehaviour
{
    Bounds combinedBounds;
    bool firstRenderer = false;

    void Update()
    {
        firstRenderer = false;
        FindRendererOnChild(transform);
        GetComponent<Enemy>().CombinedBounds = combinedBounds;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(combinedBounds.center, combinedBounds.extents * 2);
    }

    void FindRendererOnChild(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Debug.Log(child.name);
            if (child.TryGetComponent(out Renderer renderer))
            {
                if (!firstRenderer)
                {
                    combinedBounds = renderer.bounds;
                    firstRenderer = true;
                }

                combinedBounds.Encapsulate(renderer.bounds);
                FindRendererOnChild(child);
            }
        }
    }
}
