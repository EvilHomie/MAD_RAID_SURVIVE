using System;
using UnityEngine;

public class GameFlowService : MonoBehaviour
{
    public Action CustomUpdate;
    public Action CustomFixedUpdate;
    void Update()
    {
        CustomUpdate?.Invoke();
    }

    void FixedUpdate()
    {
        CustomFixedUpdate?.Invoke();
    }
}
