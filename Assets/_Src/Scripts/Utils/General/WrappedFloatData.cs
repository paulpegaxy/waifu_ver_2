using System;

public class WrappedFloatData
{
    public Action<float> onValueChanged;
    public float Value
    {
        get => value;
        set
        {
            this.value = value;
            onValueChanged?.Invoke(this.value);
        }
    }
    private float value;

    public static WrappedFloatData operator +(WrappedFloatData value1, float value2)
    {
        value1.Value += value2;
        return value1;
    }
    public static WrappedFloatData operator -(WrappedFloatData value1, float value2)
    {
        value1.Value -= value2;
        return value1;
    }

    public static WrappedFloatData operator --(WrappedFloatData value1)
    {
        value1.Value--;
        return value1;
    }
    public static WrappedFloatData operator ++(WrappedFloatData value1)
    {
        value1.Value++;
        return value1;
    }

    public static bool operator ==(WrappedFloatData value1, int value2)
    {
        return value1 != null && value1.Value == value2;
    }
    public static bool operator !=(WrappedFloatData value1, int value2)
    {
        return value1 != null && value1.Value != value2;
    }
    public static bool operator <=(WrappedFloatData value1, int value2)
    {
        return value1 != null && value1.Value <= value2;
    }
    public static bool operator >=(WrappedFloatData value1, int value2)
    {
        return value1 != null && value1.Value >= value2;
    }
    public static bool operator >(WrappedFloatData value1, int value2)
    {
        return value1 != null && value1.Value > value2;
    }
    public static bool operator <(WrappedFloatData value1, int value2)
    {
        return value1 != null && value1.Value > value2;
    }
}