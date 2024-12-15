using System;

public static class ExtensionTime
{
    private static readonly DateTime s_Epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    
    public static long ToUnixTimeSeconds(this DateTime date) =>
        Convert.ToInt64((date.ToUniversalTime() - s_Epoch).TotalSeconds);

    public static DateTime TimeSecondsToLocalDateTime(this long unixTimeInSeconds) =>
        s_Epoch.AddSeconds(unixTimeInSeconds).ToLocalTime();
    
    public static string SecondToString(this float time)
    {
        var minute = (int) time / 60;
        var second = (int) time % 60;
        var miliSecond = time - (int) time;
        return $"{minute:00}:{second:00}:{miliSecond * 1000:000}";
    }
    
    public static float GetUtcDailyRefreshTime()
    {
        var utcNowTimeSpan = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        var hour = 23 - utcNowTimeSpan.Hours;
        var min = 59 - utcNowTimeSpan.Minutes;
        var sec = 60 - utcNowTimeSpan.Seconds;
        return hour * 3600 + min * 60 + sec;
    }

    public static string CalculateRefreshTime(float time)
    {
        var timeSpan = TimeSpan.FromSeconds(time);
        return string.Format("Refresh in <color={3}>{0}:{1}:{2}</color>", timeSpan.Hours.ToString("00"),
            timeSpan.Minutes.ToString("00"), timeSpan.Seconds.ToString("00"), "#8BED33");
    }
    
    public static string CalculateRefreshTimeDefault(float time)
    {
        var timeSpan = TimeSpan.FromSeconds(time);
        return $"{timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
    }

    public static string CalculateRefreshTimeHourLeft(int time)
    {
        var timeSpan = TimeSpan.FromSeconds(time);
        if (timeSpan.Hours > 0)
            return $"{timeSpan.Hours:00} hours left";
        return $"{timeSpan.Minutes:00} minutes left";
    }

    public static string TimeSpanToString(this TimeSpan timeSpan)
    {
        if (timeSpan.Hours > 0)
            return $"{timeSpan.Hours:00} hours";
        if (timeSpan.Minutes > 0)
            return $"{timeSpan.Minutes:00} minutes";
        return $"{timeSpan.Seconds:00} seconds";
    }
    
    public static string DateTimeToString(DateTime endDate)
    {
        if (endDate.Day > 1)
            return endDate.ToString("dd/MM/yyyy");

        var timeSpan = ServiceTime.GetTimeSpanEndData(endDate.ToUnixTimeSeconds());
        if (timeSpan.Hours > 0)
            return $"{timeSpan.Hours:00} hours";
        if (timeSpan.Minutes > 0)
            return $"{timeSpan.Minutes:00} minutes";
        return $"{timeSpan.Seconds:00} seconds";
    }

    public static DateTime UnixTimeStampToDateTime( double unixTimeStamp )
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
        return dateTime;
    }
    
    [Obsolete]
    public static int GetUtcNowTimestamp()
    {
        return (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    }
}
