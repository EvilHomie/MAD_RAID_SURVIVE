using UnityEngine;

namespace EditorHelpers
{
    [ExecuteInEditMode]
    public class FindCombineBounds : MonoBehaviour
    {
        Bounds combinedBounds;
        bool firstRenderer = false;

        void Update()
        {
            combinedBounds.center = Vector3.zero;
            combinedBounds.extents = Vector3.zero;
            firstRenderer = false;
            if (transform.TryGetComponent(out Renderer parentRenderer))
            {
                combinedBounds = parentRenderer.bounds;
                firstRenderer = true;
            }
            FindRendererOnChild(transform);
            if (transform.TryGetComponent(out IRendererBounds bounds))
            {
                bounds.CombinedBounds = combinedBounds;
            }
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
                if (child.TryGetComponent(out Renderer renderer))
                {

                    if (!firstRenderer)
                    {
                        combinedBounds = renderer.bounds;
                        firstRenderer = true;
                    }

                    combinedBounds.Encapsulate(renderer.bounds);
                }
                FindRendererOnChild(child);
            }
        }
    }
}

