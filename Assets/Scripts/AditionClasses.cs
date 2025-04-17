
using System;
using UnityEngine;

public class AditionClasses
{

}


[Serializable]
public struct AreaZone
{
    public float XMin;
    public float XMax;
    public float ZMin;
    public float ZMax;

    public AreaZone(Vector4 positions)
    {
        XMin = positions.x;
        XMax = positions.y;
        ZMin = positions.z;
        ZMax = positions.w;
    }
}

public enum ObjectSize
{
    Large,
    Medium,
    Small
}

public static class Vector2Extension
{
    public static float RandomValue(this Vector2 vector2)
    {
        return UnityEngine.Random.Range(vector2.x, vector2.y);
    }
}

