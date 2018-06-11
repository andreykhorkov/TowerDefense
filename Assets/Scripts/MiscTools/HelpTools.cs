using UnityEngine;
using Random = System.Random;

public static class HelpTools
{
    public static Random random;

    static HelpTools()
    {
        random = new Random();
    }

    public static bool Approximately(float a, float b, float tolerance = 0.000001f)
    {
        return Mathf.Abs(a - b) < tolerance;
    }

    public static double NextRandomDouble(double min, double max)
    {
        return min + (max - min) * random.NextDouble();
    }

    public static void ChangeLayersRecursively(Transform trans, int layer)
    {
        trans.gameObject.layer = layer;
        foreach (Transform child in trans)
        {
            ChangeLayersRecursively(child.transform, layer);
        }
    }
}
