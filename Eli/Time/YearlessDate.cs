namespace Eli.Time;

public readonly struct YearlessDate
{
    public int Month { get; }
    public int Day { get; }

    public YearlessDate(int month, int day)
    {
        Month = month;
        Day = day;
    }

    public YearlessDate(System.DateTime dateTime)
    {
        Month = dateTime.Month;
        Day = dateTime.Day;
    }

    public override bool Equals(object obj)
    {
        var yearlessDate = (YearlessDate)obj;
        return Month == yearlessDate.Month && Day == yearlessDate.Day;
    }

    public static bool operator ==(YearlessDate left, YearlessDate right) => left.Equals(right);

    public static bool operator !=(YearlessDate left, YearlessDate right) => !(left == right);

    public override int GetHashCode() => base.GetHashCode();
}
