using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Templates;
using System.Collections.Generic;

namespace Eli.Avalonia.Plot;

public partial class HeatmapControl : UserControl
{
    public static readonly StyledProperty<IEnumerable<IEnumerable<object>>?> ItemsProperty =
        AvaloniaProperty.Register<HeatmapControl, IEnumerable<IEnumerable<object>>?>(nameof(Items));

    public IEnumerable<IEnumerable<object>>? Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public static readonly StyledProperty<DataTemplate?> CellTemplateProperty =
        AvaloniaProperty.Register<HeatmapControl, DataTemplate?>(nameof(CellTemplate));

    public DataTemplate? CellTemplate
    {
        get => GetValue(CellTemplateProperty);
        set => SetValue(CellTemplateProperty, value);
    }

    public HeatmapControl() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}