using System;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class AditionalClasses
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

    public float GetRandomZPos()
    {
        return Random.Range(ZMin, ZMax);
    }
    public float GetRandomXPos()
    {
        return Random.Range(XMin, XMax);
    }
}

public enum ObjectSize
{
    Large,
    Medium,
    Small
}

public enum RotateDir
{
    Clockwise,
    CounterClockwise

}

public static class Vector2Extension
{
    public static float RandomValue(this Vector2 vector2)
    {
        return UnityEngine.Random.Range(vector2.x, vector2.y);
    }
}

public static class CancellationTokenSourceExtension
{
    public static CancellationTokenSource Create(this CancellationTokenSource cts)
    {
        cts?.Dispose();
        return new CancellationTokenSource();
    }

    public static void CancelAndDispose(this CancellationTokenSource cts)
    {
        cts?.Cancel();
        cts?.Dispose();
    }
}

public interface IInput
{
    public Action<Vector2> OnMoveCursor { get; set; }
    public Action OnPress { get; set; }
}

public interface IRendererBounds
{
    [SerializeField] Bounds CombinedBounds { get; set; }
}

