namespace Application.Extensions;

public static class DateOnlyExtensions
{
    public static DateTime UtcMax(this DateOnly dateOnly)
    {
        return dateOnly.ToDateTime(TimeOnly.MaxValue).ToUniversalTime();
    }
    
    public static DateTime UtcMin(this DateOnly dateOnly)
    {
        return dateOnly.ToDateTime(TimeOnly.MinValue).ToUniversalTime();
    }
}