namespace Common.Wpf
{
    public enum LineStyle
    {
        None,
        Solid,
        Dash,
        Dot,
        //DashDot,
        //DashDashDot,
        //DashDotDot,
        //DashDashDotDot,
        //LongDash,
        //LongDashDot,
        //LongDashDotDot,
        //Undefined
    }

    public static class LineStyleExtensions
    {
        public static OxyPlot.LineStyle ToOxyPlotLineStyle(this LineStyle lineStyle)
        {
            switch(lineStyle)
            {
                case LineStyle.Solid:
                    return OxyPlot.LineStyle.Solid;
                case LineStyle.Dash:
                    return OxyPlot.LineStyle.Dash;
                case LineStyle.Dot:
                    return OxyPlot.LineStyle.Dot;
                default:
                    return OxyPlot.LineStyle.None;
            }
        }
    }
}
