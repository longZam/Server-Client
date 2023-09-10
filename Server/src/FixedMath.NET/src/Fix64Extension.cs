namespace FixedMath.NET;

public static class Fix64Extension
{
    public static Fix64 Clamp(Fix64 value, Fix64 min, Fix64 max)
    {
        if (min > value)
            return min;
        else if (max < value)
            return max;
        else
            return value;
    }

    public static Fix64 Clamp01(Fix64 value)
    {
        return Clamp(value, Fix64.Zero, Fix64.One);
    }

    public static Fix64 Max(params Fix64[] values)
    {
        Fix64 result = Fix64.MinValue;

        for (int i = 0; i < values.Length; i++)
            if (result < values[i])
                result = values[i];
        
        return result;
    }

    public static Fix64 Min(params Fix64[] values)
    {
        Fix64 result = Fix64.MaxValue;

        for (int i = 0; i < values.Length; i++)
            if (result > values[i])
                result = values[i];
        
        return result;
    }
}