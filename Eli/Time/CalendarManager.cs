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

    private static readonly Lazy<ConcurrentDictionary<int, DateOnly>> EasterSundayCache = new(() => new ConcurrentDictionary<int, DateOnly>());

    private static bool IsThursday(DateOnly date) => date.DayOfWeek == DayOfWeek.Thursday;
    private static bool IsFriday(DateOnly date) => date.DayOfWeek == DayOfWeek.Friday;
    private static bool IsMonday(DateOnly date) => date.DayOfWeek == DayOfWeek.Monday;
    private static bool IsWeekend(DateOnly date) => date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    public static bool IsNyseOpen(DateOnly date)
    {
        if(IsWeekend(date) || IsObservedNyseHoliday(date) || MiscellaneousNyseClosedDates.Contains(date)) return false;
        else return true;
    }

    private static bool IsObservedNyseHoliday(DateOnly date)
    {
        var nthWeekDay = (int)Math.Ceiling(date.Day / 7.0d);

        if((date.Month == 12 && date.Day == 31 && IsFriday(date)) ||
            (date.Month == 1 && date.Day == 1 && !IsWeekend(date)) ||
            (date.Month == 1 && date.Day == 2 && IsMonday(date))) return true;

        if(date.Month == 1 && IsMonday(date) && nthWeekDay == 3) return true;
        if(date.Month == 2 && IsMonday(date) && nthWeekDay == 3) return true;

        if((IsGoodFriday(date.AddDays(-1)) && IsFriday(date)) ||
            (IsGoodFriday(date) && !IsWeekend(date)) ||
            (IsGoodFriday(date.AddDays(1)) && IsMonday(date))) return true;

        if(date.Month == 5 && IsMonday(date) && date.AddDays(7).Month == 6) return true;

        if((date.Month == 6 && date.Day == 18 && IsFriday(date)) ||
            (date.Month == 6 && date.Day == 19 && !IsWeekend(date)) ||
            (date.Month == 6 && date.Day == 20 && IsMonday(date))) return true;

        if((date.Month == 7 && date.Day == 3 && IsFriday(date)) ||
            (date.Month == 7 && date.Day == 4 && !IsWeekend(date)) ||
            (date.Month == 7 && date.Day == 5 && IsMonday(date))) return true;

        if(date.Month == 9 && IsMonday(date) && nthWeekDay == 1) return true;

        if(date.Month == 11 && IsThursday(date) && nthWeekDay == 4) return true;

        return (date.Month == 12 && date.Day == 24 && IsFriday(date)) ||
               (date.Month == 12 && date.Day == 25 && !IsWeekend(date)) ||
               (date.Month == 12 && date.Day == 26 && IsMonday(date));
    }

    public static bool IsFederalHoliday(DateOnly date)
    {
        var nthWeekDay = (int)Math.Ceiling(date.Day / 7.0d);
        var dayName = date.DayOfWeek;
        var isThursday = dayName == DayOfWeek.Thursday;
        var isFriday = dayName == DayOfWeek.Friday;
        var isMonday = dayName == DayOfWeek.Monday;
        var isWeekend = dayName is DayOfWeek.Saturday or DayOfWeek.Sunday;

        if((date.Month == 12 && date.Day == 31 && isFriday) ||
            (date.Month == 1 && date.Day == 1 && !isWeekend) ||
            (date.Month == 1 && date.Day == 2 && isMonday)) return true;

        if(date.Month == 1 && isMonday && nthWeekDay == 3) return true;
        if(date.Month == 2 && isMonday && nthWeekDay == 3) return true;
        if(date.Month == 5 && isMonday && date.AddDays(7).Month == 6) return true;

        if((date.Month == 6 && date.Day == 18 && isFriday) ||
            (date.Month == 6 && date.Day == 19 && !isWeekend) ||
            (date.Month == 6 && date.Day == 20 && isMonday)) return true;

        if((date.Month == 7 && date.Day == 3 && isFriday) ||
            (date.Month == 7 && date.Day == 4 && !isWeekend) ||
            (date.Month == 7 && date.Day == 5 && isMonday)) return true;

        if(date.Month == 9 && isMonday && nthWeekDay == 1) return true;
        if(date.Month == 10 && isMonday && nthWeekDay == 2) return true;

        if((date.Month == 11 && date.Day == 10 && isFriday) ||
            (date.Month == 11 && date.Day == 11 && !isWeekend) ||
            (date.Month == 11 && date.Day == 12 && isMonday)) return true;

        if(date.Month == 11 && isThursday && nthWeekDay == 4) return true;

        return (date.Month == 12 && date.Day == 24 && isFriday) ||
               (date.Month == 12 && date.Day == 25 && !isWeekend) ||
               (date.Month == 12 && date.Day == 26 && isMonday);
    }

    public static DateOnly GetEasterSunday(int year)
    {
        return EasterSundayCache.Value.GetOrAdd(year, y =>
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
    }

    public static bool IsGoodFriday(DateOnly date) => date == GetEasterSunday(date.Year).AddDays(-2);
}
