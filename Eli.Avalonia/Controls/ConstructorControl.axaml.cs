using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Eli.Text;
using DynamicData;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Eli.Avalonia.Controls;

public partial class ConstructorControl : UserControl
{
    private ConstructorInfo? _constructorInfo;
    public ConstructorInfo? ConstructorInfo
    {
        get => _constructorInfo;
        set => SetAndRaise(ConstructorInfoProperty, ref _constructorInfo, value);
    }
    public static readonly DirectProperty<ConstructorControl, ConstructorInfo?> ConstructorInfoProperty =
        AvaloniaProperty.RegisterDirect<ConstructorControl, ConstructorInfo?>(nameof(ConstructorInfo), o => o.ConstructorInfo, (o, v) => { o.ConstructorInfo = v; OnConstructorChanged(o, v); });

    public object? ConstructedObject
    {
        get => GetValue(ConstructedObjectProperty);
        set => SetValue(ConstructedObjectProperty, value);
    }
    public static readonly DirectProperty<ConstructorControl, object?> ConstructedObjectProperty =
        AvaloniaProperty.RegisterDirect<ConstructorControl, object?>(nameof(ConstructedObject), o => o.ConstructedObject, (o, v) => o.ConstructedObject = v);

    public ObservableCollection<ParameterViewModel> Parameters { get; } = new();

    public static void OnConstructorChanged(object o, object? v)
    {
        var value = v as ConstructorInfo;
        if(o is not ConstructorControl control || value == null) return;
        if(control.ConstructorInfo == null) return;
        control.Parameters.Clear();
        control.Parameters.AddRange(control.ConstructorInfo.GetParameters().Select(p => new ParameterViewModel { Name = p.Name ?? "Name Not Found" }));
    }

    public ConstructorControl() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    private void InstantiateClick(object sender, RoutedEventArgs e)
    {
        var parameterValues = Parameters?.Select(p => Convert.ChangeType(p.Value, typeof(object))).ToArray();

        try
        {
            ConstructedObject = ConstructorInfo?.Invoke(parameterValues);
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
    public string DisplayName => CaseConverter.ToSpacedPascalCase(Name);
    public object Value { get; init; } = "";
}