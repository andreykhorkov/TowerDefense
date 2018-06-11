using UnityEngine;

public static class HelpTools
{
    public static bool Approximately(float a, float b, float tolerance = 0.000001f)
    {
        return Mathf.Abs(a - b) < tolerance;
    }
}
