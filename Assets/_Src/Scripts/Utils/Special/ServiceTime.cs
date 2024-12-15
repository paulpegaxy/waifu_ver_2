using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Runtime;

public class ServiceTime
{
    public static DateTimeOffset ServerTime;
    public static TimeSpan TimeOffset;

    public static DateTimeOffset LocalTime => DateTimeOffset.UtcNow;
    public static DateTimeOffset CurrentTime => ServerTime + TimeOffset;
    public static DateTime TodayAtMidnight => CurrentTime.Date;
    public static DateTime NextDayAtMidnight => TodayAtMidnight.AddDays(1);
    public static DateTime FirstDayOfWeek => TodayAtMidnight.AddDays(-(int)CurrentTime.DayOfWeek + 1);
    public static DateTime NextDayOfWeek => FirstDayOfWeek.AddDays(7);
    public static long CurrentUnixTime => CurrentTime.ToUnixTimeSeconds();
    public static long TotalSecondsInDay => 86400;

    public static async UniTask Init()
    {
        ServerTime = await GetServerTime();

        static void Loop(object obj)
        {
            TimeOffset = TimeOffset.Add(TimeSpan.FromSeconds(1));
        };

        var cts = new CancellationTokenSource();
        PlayerLoopTimer.StartNew(TimeSpan.FromSeconds(1), true, DelayType.Realtime, PlayerLoopTiming.Update, cts.Token, Loop, null);
    }

    public static async UniTask Refresh()
    {
        ServerTime = await GetServerTime();
        TimeOffset = TimeSpan.Zero;
    }

    private static async UniTask<DateTimeOffset> GetServerTime()
    {
        try
        {
            var api = FactoryApi.Get<ApiCommon>();
            var data = await api.GetServerInfo();

            return DateTimeOffset.FromUnixTimeSeconds(data.utc_time);
        }
        catch
        {
            return DateTimeOffset.UtcNow;
        }
    }

    public static long GetTimeRemain(DateTime endDate)
    {
        var value = endDate.ToUnixTimeSeconds() - CurrentUnixTime;
        return value > 0 ? value : 0;
    }

    public static TimeSpan GetTimeSpanEndData(long endTime)
    {
        var timeRemain = endTime - CurrentUnixTime;
        return TimeSpan.FromSeconds(timeRemain);
    }

    public static long GetTimeRemain(long endTime)
    {
        return endTime - CurrentUnixTime;
    }
}