using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Avalonia.Mvvm
{
    public interface IAlertable
    {
        void Alert(string propertyName, string alertType = "", object? data = null);
    }
}
