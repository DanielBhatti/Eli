using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Eli.Time;

public static class CalendarManager
{
    private static readonly HashSet<DateOnly> MiscellaneousNyseClosedDates = new()
    {
        new(2018, 12, 5),
        new(2012, 10, 29),
        new(2012, 10, 30),
        new(2007, 1, 2),
        new(2004, 6, 11),
        new(2001, 9, 11),
        new(2001, 9, 12),
        new(2001, 9, 13),
        new(2001, 9, 14),
        new(1994, 4, 27),
        new(1985, 9, 27),
        new(1980, 11, 4),
        new(1977, 7, 14),
        new(1976, 11, 2),
        new(1973, 1, 25),
        new(1972, 12, 28),
        new(1970, 2, 23)
    };

    private static readonly Lazy<ConcurrentDictionary<int, DateOnly>> EasterSundayCache =
        new(() => new ConcurrentDictionary<int, DateOnly>());

    public static bool IsNyseOpen(DateOnly date) =>
        !IsWeekend(date) &&
        !IsNyseHoliday(date) &&
        !MiscellaneousNyseClosedDates.Contains(date);

    public static bool IsFederalHoliday(DateOnly date) =>
        IsNewYearsDay(date) ||
        IsMartinLutherKingJrDay(date) ||
        IsPresidentsDay(date) ||
        IsMemorialDay(date) ||
        IsJuneteenth(date) ||
        IsIndependenceDay(date) ||
        IsLaborDay(date) ||
        IsColumbusDay(date) ||
        IsVeteransDay(date) ||
        IsThanksgivingDay(date) ||
        IsChristmasDay(date);

    public static bool IsNyseHoliday(DateOnly date) =>
        IsNewYearsDay(date) ||
        IsMartinLutherKingJrDay(date) ||
        IsPresidentsDay(date) ||
        IsGoodFriday(date) ||
        IsMemorialDay(date) ||
        IsJuneteenth(date) ||
        IsIndependenceDay(date) ||
        IsLaborDay(date) ||
        IsThanksgivingDay(date) ||
        IsChristmasDay(date);

    public static bool IsNewYearsDay(DateOnly date) =>
        (date.Month == 1 && date.Day == 1 && !IsWeekend(date)) ||
        (date.Month == 12 && date.Day == 31 && IsFriday(date)) ||
        (date.Month == 1 && date.Day == 2 && IsMonday(date));

    public static bool IsMartinLutherKingJrDay(DateOnly date) => IsNthMonday(date, month: 1, nth: 3);

    public static bool IsPresidentsDay(DateOnly date) => IsNthMonday(date, month: 2, nth: 3);

    public static bool IsMemorialDay(DateOnly date) => IsLastMondayInMay(date);

    public static bool IsJuneteenth(DateOnly date) => IsObservedFixedHoliday(date, month: 6, day: 19);

    public static bool IsIndependenceDay(DateOnly date) => IsObservedFixedHoliday(date, month: 7, day: 4);

    public static bool IsLaborDay(DateOnly date) => IsNthMonday(date, month: 9, nth: 1);

    public static bool IsColumbusDay(DateOnly date) => IsNthMonday(date, month: 10, nth: 2);

    public static bool IsVeteransDay(DateOnly date) => IsObservedFixedHoliday(date, month: 11, day: 11);

    public static bool IsThanksgivingDay(DateOnly date) => IsNthThursday(date, month: 11, nth: 4);

    public static bool IsChristmasDay(DateOnly date) => IsObservedFixedHoliday(date, month: 12, day: 25);

    public static bool IsGoodFriday(DateOnly date)
    {
        var goodFriday = GetEasterSunday(date.Year).AddDays(-2);

        if(date == goodFriday && !IsWeekend(date)) return true;
        if(date == goodFriday.AddDays(-1) && IsFriday(date)) return true;
        if(date == goodFriday.AddDays(1) && IsMonday(date)) return true;

        return false;
    }

    public static DateOnly GetEasterSunday(int year) =>
    EasterSundayCache.Value.GetOrAdd(year, y =>
    {
        var g = y % 19;
        var c = y / 100;
        var h = (c - c / 4 - (8 * c + 13) / 25 + 19 * g + 15) % 30;
        var i = h - h / 28 * (1 - h / 28 * (29 / (h + 1)) * ((21 - g) / 11));

        var day = i - (y + y / 4 + i + 2 - c + c / 4) % 7 + 28;
        var month = 3;

        if(day > 31)
        {
            month++;
            day -= 31;
        }

        return new DateOnly(y, month, day);
    });

    public static bool IsWeekend(DateOnly date) => date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    private static bool IsMonday(DateOnly date) => date.DayOfWeek == DayOfWeek.Monday;

    private static bool IsThursday(DateOnly date) => date.DayOfWeek == DayOfWeek.Thursday;

    private static bool IsFriday(DateOnly date) => date.DayOfWeek == DayOfWeek.Friday;

    private static int NthWeekdayInMonth(DateOnly date) => (date.Day + 6) / 7; // 1..5

    private static bool IsNthMonday(DateOnly date, int month, int nth) =>
        date.Month == month &&
        IsMonday(date) &&
        NthWeekdayInMonth(date) == nth;

    private static bool IsNthThursday(DateOnly date, int month, int nth) =>
        date.Month == month &&
        IsThursday(date) &&
        NthWeekdayInMonth(date) == nth;

    private static bool IsLastMondayInMay(DateOnly date) =>
        date.Month == 5 &&
        IsMonday(date) &&
        date.AddDays(7).Month == 6;

    private static bool IsObservedFixedHoliday(DateOnly date, int month, int day) =>
        (date.Month == month && date.Day == day && !IsWeekend(date)) ||
        (date.Month == month && date.Day == day - 1 && IsFriday(date)) ||
        (date.Month == month && date.Day == day + 1 && IsMonday(date));
}
