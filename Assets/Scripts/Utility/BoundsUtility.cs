using UnityEngine;

public static class BoundsUtility
{
    public static Vector3 GetRandomPointInBounds(Bounds bounds, float boundsPadding = 0f)
    {
        return new Vector3(
                Random.Range(bounds.min.x + boundsPadding, bounds.max.x - boundsPadding),
                Random.Range(bounds.min.y + boundsPadding, bounds.max.y - boundsPadding),
                Random.Range(bounds.min.z + boundsPadding, bounds.max.z - boundsPadding));
    }
}
