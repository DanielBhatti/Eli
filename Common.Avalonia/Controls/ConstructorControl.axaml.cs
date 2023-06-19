using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Avalonia.Controls;

public partial class ConstructorControl : UserControl
{
    public ConstructorInfo Constructor
    {
        get => GetValue(ConstructorProperty);
        set => SetValue(ConstructorProperty, value);
    }
    public static readonly DirectProperty<ConstructorControl, ConstructorInfo> ConstructorProperty =
        AvaloniaProperty.RegisterDirect<ConstructorControl, ConstructorInfo>(nameof(Constructor), o => o.Constructor, (o, v) => { o.Constructor = v; OnConstructorChanged(o, v); });

    public object? ConstructedObject
    {
        get => GetValue(ConstructedObjectProperty);
        set => SetValue(ConstructedObjectProperty, value);
    }
    public static readonly DirectProperty<ConstructorControl, object?> ConstructedObjectProperty =
        AvaloniaProperty.RegisterDirect<ConstructorControl, object?>(nameof(ConstructedObject), o => o.ConstructedObject, (o, v) => o.ConstructedObject = v);

    public List<ParameterViewModel> ParameterList { get; set; } = new();

    private static void OnConstructorChanged(object o, object v)
    {
        var value = v as ConstructorInfo;
        if(o is not ConstructorControl control || value == null) return;
        control.ParameterList = control.Constructor.GetParameters().Select(p => new ParameterViewModel { Name = p.Name ?? "N/A" }).ToList();
    }

    public ConstructorControl() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    private void InstantiateClick(object sender, RoutedEventArgs e)
    {
        var parameterValues = ParameterList.Select(p => Convert.ChangeType(p.Value, typeof(string))).ToArray();

        try
        {
            ConstructedObject = Constructor.Invoke(parameterValues);
        }
        catch
        {
            ConstructedObject = null;
        }
    }
}

public class ParameterViewModel
{
    public required string Name { get; init; }
    public object Value { get; init; } = "";
}