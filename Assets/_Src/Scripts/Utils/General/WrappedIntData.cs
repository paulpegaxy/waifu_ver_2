using System;

public class WrappedIntData
{
    public Action<int> onValueChanged;
    public int Value
    {
        get => value;
        set
        {
            this.value = value;
            onValueChanged?.Invoke(this.value);
        }
    }
    private int value;

    public static WrappedIntData operator +(WrappedIntData value1, int value2)
    {
        value1.Value += value2;
        return value1;
    }
    public static WrappedIntData operator -(WrappedIntData value1, int value2)
    {
        value1.Value -= value2;
        return value1;
    }

    public static WrappedIntData operator --(WrappedIntData value1)
    {
        value1.Value--;
        return value1;
    }
    public static WrappedIntData operator ++(WrappedIntData value1)
    {
        value1.Value++;
        return value1;
    }

    public static bool operator ==(WrappedIntData value1, int value2)
    {
        return value1 != null && value1.Value == value2;
    }
    public static bool operator !=(WrappedIntData value1, int value2)
    {
        return value1 != null && value1.Value != value2;
    }
    public static bool operator <=(WrappedIntData value1, int value2)
    {
        return value1 != null && value1.Value <= value2;
    }
    public static bool operator >=(WrappedIntData value1, int value2)
    {
        return value1 != null && value1.Value >= value2;
    }
    public static bool operator >(WrappedIntData value1, int value2)
    {
        return value1 != null && value1.Value > value2;
    }
    public static bool operator <(WrappedIntData value1, int value2)
    {
        return value1 != null && value1.Value > value2;
    }
}