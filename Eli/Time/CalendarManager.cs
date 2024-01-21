using System;

namespace Eli.Time;

public class CalendarManager
{
    public CalendarManager() { }

    public bool IsMarketCloseDay(DateTime date)
    {
        // to ease typing
        var nthWeekDay = (int)Math.Ceiling(date.Day / 7.0d);
        var dayName = date.DayOfWeek;
        var isThursday = dayName == DayOfWeek.Thursday;
        var isFriday = dayName == DayOfWeek.Friday;
        var isMonday = dayName == DayOfWeek.Monday;
        var isWeekend = dayName is DayOfWeek.Saturday or DayOfWeek.Sunday;

        // New Years Day (Jan 1, or preceding Friday/following Monday if weekend)
        if((date.Month == 12 && date.Day == 31 && isFriday) ||
            (date.Month == 1 && date.Day == 1 && !isWeekend) ||
            (date.Month == 1 && date.Day == 2 && isMonday))
        {
            return true;
        }

        // MLK day (3rd monday in January)
        if(date.Month == 1 && isMonday && nthWeekDay == 3)
        {
            return true;
        }

        // President’s Day (3rd Monday in February)
        if(date.Month == 2 && isMonday && nthWeekDay == 3)
        {
            return true;
        }

        // Good Friday (based on the Lunar Calendar)
        if((IsGoodFriday(date.AddDays(-1)) && isFriday) ||
            (IsGoodFriday(date) && !isWeekend) ||
            (IsGoodFriday(date.AddDays(1)) && isMonday))
        {
            return true;
        }

        // Memorial Day (Last Monday in May)
        if(date.Month == 5 && isMonday && date.AddDays(7).Month == 6)
        {
            return true;
        }

        // Juneteenth (June 19, or preceding Friday/following Monday if weekend)
        if((date.Month == 6 && date.Day == 18 && isFriday) ||
            (date.Month == 6 && date.Day == 19 && !isWeekend) ||
            (date.Month == 6 && date.Day == 20 && isMonday))
        {
            return true;
        }

        // Independence Day (July 4, or preceding Friday/following Monday if weekend)
        if((date.Month == 7 && date.Day == 3 && isFriday) ||
            (date.Month == 7 && date.Day == 4 && !isWeekend) ||
            (date.Month == 7 && date.Day == 5 && isMonday))
        {
            return true;
        }

        // Labor Day (1st Monday in September)
        if(date.Month == 9 && isMonday && nthWeekDay == 1)
        {
            return true;
        }

        // Thanksgiving Day (4th Thursday in November)
        if(date.Month == 11 && isThursday && nthWeekDay == 4)
        {
            return true;
        }

        // Christmas Day (December 25, or preceding Friday/following Monday if weekend))
        return (date.Month == 12 && date.Day == 24 && isFriday) ||
            (date.Month == 12 && date.Day == 25 && !isWeekend) ||
            (date.Month == 12 && date.Day == 26 && isMonday);
    }

    public bool IsFederalHoliday(DateTime date)
    {
        // to ease typing
        var nthWeekDay = (int)Math.Ceiling(date.Day / 7.0d);
        var dayName = date.DayOfWeek;
        var isThursday = dayName == DayOfWeek.Thursday;
        var isFriday = dayName == DayOfWeek.Friday;
        var isMonday = dayName == DayOfWeek.Monday;
        var isWeekend = dayName is DayOfWeek.Saturday or DayOfWeek.Sunday;

        // New Years Day (Jan 1, or preceding Friday/following Monday if weekend)
        if((date.Month == 12 && date.Day == 31 && isFriday) ||
            (date.Month == 1 && date.Day == 1 && !isWeekend) ||
            (date.Month == 1 && date.Day == 2 && isMonday))
        {
            return true;
        }

        // MLK day (3rd monday in January)
        if(date.Month == 1 && isMonday && nthWeekDay == 3)
        {
            return true;
        }

        // President’s Day (3rd Monday in February)
        if(date.Month == 2 && isMonday && nthWeekDay == 3)
        {
            return true;
        }

        // Memorial Day (Last Monday in May)
        if(date.Month == 5 && isMonday && date.AddDays(7).Month == 6)
        {
            return true;
        }

        // Juneteenth (June 19, or preceding Friday/following Monday if weekend)
        if((date.Month == 6 && date.Day == 18 && isFriday) ||
            (date.Month == 6 && date.Day == 19 && !isWeekend) ||
            (date.Month == 6 && date.Day == 20 && isMonday))
        {
            return true;
        }

        // Independence Day (July 4, or preceding Friday/following Monday if weekend)
        if((date.Month == 7 && date.Day == 3 && isFriday) ||
            (date.Month == 7 && date.Day == 4 && !isWeekend) ||
            (date.Month == 7 && date.Day == 5 && isMonday))
        {
            return true;
        }

        // Labor Day (1st Monday in September)
        if(date.Month == 9 && isMonday && nthWeekDay == 1)
        {
            return true;
        }

        // Columbus Day (2nd Monday in October)
        if(date.Month == 10 && isMonday && nthWeekDay == 2)
        {
            return true;
        }

        // Veteran’s Day (November 11, or preceding Friday/following Monday if weekend))
        if((date.Month == 11 && date.Day == 10 && isFriday) ||
            (date.Month == 11 && date.Day == 11 && !isWeekend) ||
            (date.Month == 11 && date.Day == 12 && isMonday))
        {
            return true;
        }

        // Thanksgiving Day (4th Thursday in November)
        if(date.Month == 11 && isThursday && nthWeekDay == 4)
        {
            return true;
        }

        // Christmas Day (December 25, or preceding Friday/following Monday if weekend))
        return (date.Month == 12 && date.Day == 24 && isFriday) ||
            (date.Month == 12 && date.Day == 25 && !isWeekend) ||
            (date.Month == 12 && date.Day == 26 && isMonday);
    }

    public DateTime GetEasterSunday(int year)
    {
        var g = year % 19;
        var c = year / 100;
        var h = (c - c / 4 - (8 * c + 13) / 25 + 19 * g + 15) % 30;
        var i = h - h / 28 * (1 - h / 28 * (29 / (h + 1)) * ((21 - g) / 11));

        var day = i - (year + year / 4 + i + 2 - c + c / 4) % 7 + 28;
        var month = 3;
        if(day > 31)
        {
            month++;
            day -= 31;
        }

        return new DateTime(year, month, day);
    }

    public bool IsGoodFriday(DateTime date) => date == GetEasterSunday(date.Year).AddDays(-2);
}
