using System;
using NUnit.Framework;
using Eli.Time;

namespace Eli.Test.Time;

[TestFixture]
public sealed class CalendarManagerHolidayTests
{
    private static void AssertFederalHoliday(DateOnly date) =>
        Assert.That(CalendarManager.IsFederalHoliday(date), Is.True, $"Expected Federal holiday: {date:yyyy-MM-dd}");

    private static void AssertNotFederalHoliday(DateOnly date) =>
        Assert.That(CalendarManager.IsFederalHoliday(date), Is.False, $"Expected NOT Federal holiday: {date:yyyy-MM-dd}");

    private static void AssertNyseClosed(DateOnly date) =>
        Assert.That(CalendarManager.IsNyseOpen(date), Is.False, $"Expected NYSE closed: {date:yyyy-MM-dd}");

    private static void AssertNyseOpen(DateOnly date) =>
        Assert.That(CalendarManager.IsNyseOpen(date), Is.True, $"Expected NYSE open: {date:yyyy-MM-dd}");

    private static void AssertFederalHoliday(int y, int m, int d) => AssertFederalHoliday(new DateOnly(y, m, d));
    private static void AssertNotFederalHoliday(int y, int m, int d) => AssertNotFederalHoliday(new DateOnly(y, m, d));
    private static void AssertNyseClosed(int y, int m, int d) => AssertNyseClosed(new DateOnly(y, m, d));
    private static void AssertNyseOpen(int y, int m, int d) => AssertNyseOpen(new DateOnly(y, m, d));

    [Test]
    public void NewYearsDay_Observed()
    {
        AssertFederalHoliday(2021, 1, 1);
        AssertNyseClosed(2021, 1, 1);
        AssertNotFederalHoliday(2020, 12, 31);
        AssertNyseOpen(2021, 1, 4);

        AssertFederalHoliday(2021, 12, 31);
        AssertNyseClosed(2021, 12, 31);
        AssertNotFederalHoliday(2022, 1, 3);
        AssertNyseOpen(2022, 1, 3);

        AssertFederalHoliday(2023, 1, 2);
        AssertNyseClosed(2023, 1, 2);
        AssertNotFederalHoliday(2022, 12, 30);
        AssertNyseOpen(2023, 1, 3);

        AssertNotFederalHoliday(2023, 1, 4);
        AssertNyseOpen(2023, 1, 4);
    }

    [Test]
    public void MartinLutherKingJrDay_ThirdMondayInJanuary()
    {
        AssertFederalHoliday(2023, 1, 16);
        AssertNyseClosed(2023, 1, 16);
        AssertNyseOpen(2023, 1, 17);
        AssertNotFederalHoliday(2023, 1, 9);

        AssertFederalHoliday(2024, 1, 15);
        AssertNyseClosed(2024, 1, 15);
        AssertNyseOpen(2024, 1, 16);
        AssertNotFederalHoliday(2024, 1, 16);
        AssertFederalHoliday(2025, 1, 20);
        AssertNyseClosed(2025, 1, 20);
        AssertNyseOpen(2025, 1, 21);
        AssertNotFederalHoliday(2025, 1, 13);
    }

    [Test]
    public void PresidentsDay_ThirdMondayInFebruary()
    {
        AssertFederalHoliday(2023, 2, 20);
        AssertNyseClosed(2023, 2, 20);
        AssertNyseOpen(2023, 2, 21);
        AssertNotFederalHoliday(2023, 2, 13);

        AssertFederalHoliday(2024, 2, 19);
        AssertNyseClosed(2024, 2, 19);
        AssertNyseOpen(2024, 2, 20);
        AssertNotFederalHoliday(2024, 2, 12);

        AssertFederalHoliday(2025, 2, 17);
        AssertNyseClosed(2025, 2, 17);
        AssertNyseOpen(2025, 2, 18);
        AssertNotFederalHoliday(2025, 2, 10);
    }

    [Test]
    public void GoodFriday_NyseOnly()
    {
        AssertNyseClosed(2023, 4, 7);
        AssertNotFederalHoliday(2023, 4, 7);
        AssertNyseOpen(2023, 4, 10);
        AssertNyseClosed(2024, 3, 29);
        AssertNotFederalHoliday(2024, 3, 29);
        AssertNyseOpen(2024, 4, 1);

        AssertNyseClosed(2025, 4, 18);
        AssertNotFederalHoliday(2025, 4, 18);
        AssertNyseOpen(2025, 4, 21);

        var easter2025 = CalendarManager.GetEasterSunday(2025);
        AssertNotFederalHoliday(easter2025);
        AssertNyseClosed(easter2025);
    }

    [Test]
    public void MemorialDay_LastMondayInMay()
    {
        AssertFederalHoliday(2023, 5, 29);
        AssertNyseClosed(2023, 5, 29);
        AssertNyseOpen(2023, 5, 30);
        AssertNotFederalHoliday(2023, 5, 22);

        AssertFederalHoliday(2024, 5, 27);
        AssertNyseClosed(2024, 5, 27);
        AssertNyseOpen(2024, 5, 28);
        AssertNotFederalHoliday(2024, 5, 20);

        AssertFederalHoliday(2025, 5, 26);
        AssertNyseClosed(2025, 5, 26);
        AssertNyseOpen(2025, 5, 27);
        AssertNotFederalHoliday(2025, 5, 19);
    }

    [Test]
    public void Juneteenth_Observed()
    {
        AssertFederalHoliday(2022, 6, 20);
        AssertNyseClosed(2022, 6, 20);
        AssertNyseOpen(2022, 6, 21);
        AssertNotFederalHoliday(2022, 6, 21);

        AssertFederalHoliday(2024, 6, 19);
        AssertNyseClosed(2024, 6, 19);
        AssertNyseOpen(2024, 6, 20);
        AssertNotFederalHoliday(2024, 6, 20);

        AssertFederalHoliday(2021, 6, 18);
        AssertNyseClosed(2021, 6, 18);
        AssertNyseOpen(2021, 6, 21);
        AssertNotFederalHoliday(2021, 6, 21);

        AssertFederalHoliday(2026, 6, 19);
        AssertNyseClosed(2026, 6, 19);
        AssertNyseOpen(2026, 6, 22);
        AssertNotFederalHoliday(2026, 6, 22);
    }

    [Test]
    public void IndependenceDay_Observed()
    {
        AssertFederalHoliday(2020, 7, 3);
        AssertNyseClosed(2020, 7, 3);
        AssertNyseOpen(2020, 7, 6);
        AssertNotFederalHoliday(2020, 7, 6);

        AssertFederalHoliday(2021, 7, 5);
        AssertNyseClosed(2021, 7, 5);
        AssertNyseOpen(2021, 7, 6);
        AssertNotFederalHoliday(2021, 7, 6);

        AssertFederalHoliday(2025, 7, 4);
        AssertNyseClosed(2025, 7, 4);
        AssertNyseOpen(2025, 7, 7);
        AssertNotFederalHoliday(2025, 7, 7);
    }

    [Test]
    public void LaborDay_FirstMondayInSeptember()
    {
        AssertFederalHoliday(2023, 9, 4);
        AssertNyseClosed(2023, 9, 4);
        AssertNyseOpen(2023, 9, 5);
        AssertNotFederalHoliday(2023, 9, 11);

        AssertFederalHoliday(2024, 9, 2);
        AssertNyseClosed(2024, 9, 2);
        AssertNyseOpen(2024, 9, 3);
        AssertNotFederalHoliday(2024, 9, 9);

        AssertFederalHoliday(2025, 9, 1);
        AssertNyseClosed(2025, 9, 1);
        AssertNyseOpen(2025, 9, 2);
        AssertNotFederalHoliday(2025, 9, 8);
    }

    [Test]
    public void ColumbusDay_SecondMondayInOctober_FederalOnly()
    {
        AssertFederalHoliday(2023, 10, 9);
        AssertNyseOpen(2023, 10, 9);
        AssertNotFederalHoliday(2023, 10, 16);

        AssertFederalHoliday(2024, 10, 14);
        AssertNyseOpen(2024, 10, 14);
        AssertNotFederalHoliday(2024, 10, 7);

        AssertFederalHoliday(2025, 10, 13);
        AssertNyseOpen(2025, 10, 13);
        AssertNotFederalHoliday(2025, 10, 6);
    }

    [Test]
    public void VeteransDay_Observed_FederalOnly()
    {
        AssertFederalHoliday(2023, 11, 10);
        AssertNyseOpen(2023, 11, 10);
        AssertNotFederalHoliday(2023, 11, 13);

        AssertFederalHoliday(2024, 11, 11);
        AssertNyseOpen(2024, 11, 11);
        AssertNotFederalHoliday(2024, 11, 12);

        AssertFederalHoliday(2018, 11, 12);
        AssertNyseOpen(2018, 11, 12);
        AssertNotFederalHoliday(2018, 11, 13);
    }

    [Test]
    public void Thanksgiving_FourthThursdayInNovember()
    {
        AssertFederalHoliday(2023, 11, 23);
        AssertNyseClosed(2023, 11, 23);
        AssertNyseOpen(2023, 11, 24);
        AssertNotFederalHoliday(2023, 11, 30);

        AssertFederalHoliday(2024, 11, 28);
        AssertNyseClosed(2024, 11, 28);
        AssertNyseOpen(2024, 11, 29);
        AssertNotFederalHoliday(2024, 11, 21);

        AssertFederalHoliday(2025, 11, 27);
        AssertNyseClosed(2025, 11, 27);
        AssertNyseOpen(2025, 11, 28);
        AssertNotFederalHoliday(2025, 11, 20);
    }

    [Test]
    public void ChristmasDay_Observed()
    {
        AssertFederalHoliday(2020, 12, 25);
        AssertNyseClosed(2020, 12, 25);
        AssertNyseOpen(2020, 12, 28);
        AssertNotFederalHoliday(2020, 12, 28);

        AssertFederalHoliday(2021, 12, 24);
        AssertNyseClosed(2021, 12, 24);
        AssertNyseOpen(2021, 12, 27);
        AssertNotFederalHoliday(2021, 12, 27);

        AssertFederalHoliday(2022, 12, 26);
        AssertNyseClosed(2022, 12, 26);
        AssertNyseOpen(2022, 12, 27);
        AssertNotFederalHoliday(2022, 12, 27);
    }
}
