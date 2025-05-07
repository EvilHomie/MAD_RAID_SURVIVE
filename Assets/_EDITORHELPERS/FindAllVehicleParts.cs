using System.Collections.Generic;
using UnityEngine;

namespace EditorHelpers
{
    [ExecuteInEditMode]
    public class FindAllVehicleParts : MonoBehaviour
    {
        List<AbstractVehiclePart> allVehicleParts = new();
        void Update()
        {
            allVehicleParts = new();
            GetComponent<Enemy>().AllVehicleParts = FindRendererOnChild(transform);
        }

        AbstractVehiclePart[] FindRendererOnChild(Transform parent)
        {
            foreach (Transform child in parent)
            {
                if (child.TryGetComponent(out AbstractVehiclePart vehiclePart))
                {
                    allVehicleParts.Add(vehiclePart);
                }
                FindRendererOnChild(child);
            }

            return allVehicleParts.ToArray();
        }
    }
}

