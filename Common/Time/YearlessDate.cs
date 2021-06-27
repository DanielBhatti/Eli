namespace Common.Time
{
    public struct YearlessDate
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
            YearlessDate yearlessDate = (YearlessDate)obj;
            if (this.Month == yearlessDate.Month && this.Day == yearlessDate.Day) return true;
            else return false;
        }

        public static bool operator ==(YearlessDate left, YearlessDate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(YearlessDate left, YearlessDate right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
