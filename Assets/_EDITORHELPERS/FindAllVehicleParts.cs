using System.Collections.Generic;
using UnityEngine;

namespace EditorHelpers
{
    [ExecuteInEditMode]
    public class FindAllVehicleParts : MonoBehaviour
    {
        List<AbstractVehiclePart> allVehicleParts = new();
        VehicleBody _vehicleBody;
        void Update()
        {
            allVehicleParts = new();
            GetComponent<Enemy>().AllVehicleParts = FindRendererOnChild(transform);
            GetComponent<Enemy>().VehicleBody = _vehicleBody;
        }

        AbstractVehiclePart[] FindRendererOnChild(Transform parent)
        {
            foreach (Transform child in parent)
            {
                if (child.TryGetComponent(out AbstractVehiclePart vehiclePart))
                {
                    if (vehiclePart is VehicleBody vehicleBody)
                    {
                        _vehicleBody = vehicleBody;
                    }
                    else
                    {
                        allVehicleParts.Add(vehiclePart);
                    }
                }
                FindRendererOnChild(child);
            }

            return allVehicleParts.ToArray();
        }
    }
}

