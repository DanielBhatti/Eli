using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;

namespace Common.Avalonia.DataTemplateSelectors;

internal class EnumTemplate : IDataTemplate
{
    public Control? Build(object? param)
    {
        var e = param as Enum;
        var comboBox = new ComboBox { SelectedItem = e, ItemsSource = Enum.GetNames(e.GetType()) };
        return comboBox;
    }

    public bool Match(object? data) => data is Enum;
}
