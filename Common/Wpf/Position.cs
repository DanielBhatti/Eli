using System;

namespace Common.Wpf
{
    public enum Position
    {
        Top,
        Bottom,
        Left,
        Right
    }

    public static class PositionExtensions
    {
        public static OxyPlot.Axes.AxisPosition ToOxyPlotAxisPosition(this Position position)
        {
            switch(position)
            {
                case Position.Top:
                    return OxyPlot.Axes.AxisPosition.Top;
                case Position.Bottom:
                    return OxyPlot.Axes.AxisPosition.Bottom;
                case Position.Left:
                    return OxyPlot.Axes.AxisPosition.Left;
                case Position.Right:
                    return OxyPlot.Axes.AxisPosition.Right;
                default:
                    throw new ArgumentOutOfRangeException($"Position {position} does not have a corresponding OxyPlot AxisPosition defined.");
            }
        }
    }
}
