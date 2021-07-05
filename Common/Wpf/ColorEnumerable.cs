using System;
using System.Collections.Generic;

namespace Common.Wpf
{
    public class ColorEnumerable
    {
        private Stack<Color> _previousColors;
        private Stack<Color> _nextColors;

        public ColorEnumerable(ColorEnumerableStrategy colorPickerStrategy)
        {
            switch (colorPickerStrategy)
            {
                case ColorEnumerableStrategy.Random:
                    CreateRandomNextColorsStack();
                    break;
                case ColorEnumerableStrategy.Variance:
                    CreateVarianceNextColorsStack();
                    break;
            }

            _previousColors = new Stack<Color>();
        }

        public Color Next()
        {
            if (_nextColors.Count == 0) Reset();
            Color nextColor = _nextColors.Pop();
            _previousColors.Push(nextColor);
            return nextColor;
        }

        public Color Previous()
        {
            if (_previousColors.Count == 0) return Color.Black;
            Color previousColor = _previousColors.Pop();
            _nextColors.Push(previousColor);
            return previousColor;
        }

        public void Reset()
        {
            while (_previousColors.Count > 0) _nextColors.Push(_previousColors.Pop());
        }

        private void CreateRandomNextColorsStack()
        {
            Random random = new Random();
            for (int i = 0; i < 20; i++)
            {
                int r = random.Next(0, 20);
                _nextColors.Push((Color)r);
            }
        }

        private void CreateVarianceNextColorsStack()
        {
            _nextColors = new Stack<Color>();
            _nextColors.Push(Color.Red);
            _nextColors.Push(Color.Blue);
            _nextColors.Push(Color.Green);
            _nextColors.Push(Color.HotPink);
            _nextColors.Push(Color.Indigo);
            _nextColors.Push(Color.Khaki);
            _nextColors.Push(Color.SlateBlue);
            _nextColors.Push(Color.RosyBrown);
            _nextColors.Push(Color.Sienna);
        }
    }

    public enum ColorEnumerableStrategy
    {
        Random,
        Variance
    }
}
