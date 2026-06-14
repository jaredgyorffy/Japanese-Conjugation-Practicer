using UnityEngine;

public static class RandomUtility
{
    public static float GetRandomOffset(float mean, float standardDeviation)
    {
        float u1 = Random.value;
        float u2 = Random.value;

        float z0 = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Cos(2.0f * Mathf.PI * u2);

        return mean + z0 * standardDeviation;
    }

    public static float SamplePerlinNoise(float x)
    {
        // Remapping to (-1f, 1f) from (0f, 1f)
        return (Mathf.PerlinNoise(x, 0f) - 0.5f) * 2f;
    }

    public static Vector3 SamplePerlinNoiseVector3(float x)
    {
        // Using arbitrary numbers as offsets in 1D noise to achieve coherent but random-feeling output
        // TODO: This is in the shape of a cube, normalizing would bias corners- ideally outputs points in sphere with no bias
        float xPos = SamplePerlinNoise(x);
        float yPos = SamplePerlinNoise(x + 100200.1123f);
        float zPos = SamplePerlinNoise(x + 963212356.7532f);
        return new Vector3(xPos, yPos, zPos);
    }

    public static bool PercentageChanceOfTrue(float alpha)
    {
        if (alpha < 0f || alpha > 1f)
        {
            throw new System.ArgumentOutOfRangeException(nameof(alpha), "Alpha must be within 0 and 1.");
        }
        return UnityEngine.Random.value < alpha;
    }
}
