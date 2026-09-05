public enum EasingType
{
    Linear,
    Sine,
    Cubic,
    Quad,
    Quart,
    Quint,
    Exponential,
    Circular,
    Back,
    Elastic,
    Bounce,
}

public enum InterpolationType
{
    In,
    Out,
    InOut,
}
    
[System.Serializable]
public class EasingMode
{
    public float EasingDuration = 0;
    public float TimeDurationMultiplier()
    {
        return 1 / EasingDuration;
    }
    public EasingType easingType;
    public InterpolationType interpolationType;
    public EasingMode(EasingType easingType, InterpolationType interpolationType)
    {
        this.easingType = easingType;
        this.interpolationType = interpolationType;
    }
}