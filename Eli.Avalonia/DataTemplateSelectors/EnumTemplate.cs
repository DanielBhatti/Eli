using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;
using System.Linq;

namespace Eli.Avalonia.DataTemplateSelectors;

internal class EnumTemplate : IDataTemplate
{
    public Control? Build(object? param)
    {
        if(param is not Enum e) return null;
        var comboBox = new ComboBox { SelectedValue = e, ItemsSource = Enum.GetValues(e.GetType()).Cast<Enum>().ToList() };
        return comboBox;
    }

    public bool Match(object? data) => data is Enum;
}
