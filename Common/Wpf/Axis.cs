﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Wpf
{
    public class Axis
    {
        public string Name { get; set; }
        public Position Position { get; set; }
        public AxisType AxisType { get; set; }
        public string XAxisKey { get; set; }
        public string YAxisKey { get; set; }

        public Axis(string name, Position position, AxisType axisType, string xAxisKey = null, string yAxisKey = null)
        {
            Name = name;
            Position = position;
            AxisType = axisType;
            XAxisKey = xAxisKey;
            YAxisKey = yAxisKey;
        }
    }
}