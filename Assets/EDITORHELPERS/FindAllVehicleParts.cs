using System.Collections.Generic;
using UnityEngine;

namespace EditorHelpers
{
    [ExecuteInEditMode]
    public class FindAllVehicleParts : MonoBehaviour
    {
        List<VehiclePart> allVehicleParts = new();
        void Update()
        {
            allVehicleParts = new();
            GetComponent<Enemy>().AllVehicleParts = FindRendererOnChild(transform);
        }

        VehiclePart[] FindRendererOnChild(Transform parent)
        {
            foreach (Transform child in parent)
            {
                if (child.TryGetComponent(out VehiclePart vehiclePart))
                {
                    allVehicleParts.Add(vehiclePart);
                }
                FindRendererOnChild(child);
            }

            return allVehicleParts.ToArray();
        }
    }
}

