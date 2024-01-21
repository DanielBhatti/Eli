using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Linq;

namespace Eli.Avalonia.Controls;

public partial class SheetControl : UserControl
{
    private readonly Grid _mainGrid;

    public static readonly StyledProperty<int> RowCountProperty = AvaloniaProperty.Register<SheetControl, int>(nameof(RowCount));
    public int RowCount
    {
        get => GetValue(RowCountProperty);
        set => SetValue(RowCountProperty, value);
    }

    public static readonly StyledProperty<int> ColumnCountProperty = AvaloniaProperty.Register<SheetControl, int>(nameof(ColumnCount));
    public int ColumnCount
    {
        get => GetValue(ColumnCountProperty);
        set => SetValue(ColumnCountProperty, value);
    }

    //public CellControl SelectedCellControl { get => _mainGrid}

    public SheetControl()
    {
        InitializeComponent();
        _mainGrid = this.FindControl<Grid>(nameof(MainGrid))!;

        var n = 100;
        var m = 10;

        for(var j = 0; j < m; j++)
        {
            AddColumn();
        }

        for(var i = 0; i < n; i++)
        {
            AddRow();
        }
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    public void AddRow()
    {
        RowDefinition row = new();
        _mainGrid.RowDefinitions.Add(row);
        for(var i = 0; i < ColumnCount; i++)
        {
            CellControl cell = new();
            Grid.SetRow(cell, RowCount);
            Grid.SetColumn(cell, i);
            _mainGrid.Children.Add(cell);
        }
        RowCount += 1;
    }

    public void AddColumn()
    {
        ColumnDefinition column = new();
        _mainGrid.ColumnDefinitions.Add(column);
        for(var i = 0; i < RowCount; i++)
        {
            CellControl cell = new();
            Grid.SetColumn(cell, ColumnCount);
            Grid.SetRow(cell, i);
            _mainGrid.Children.Add(cell);
        }
        ColumnCount += 1;
    }

    public void DownRow()
    {
        foreach(var control in _mainGrid.Children.Cast<CellControl>())
        {
            if(control.IsKeyboardFocusWithin)
            {
                var rowIndex = Grid.GetRow(control);
                var columnIndex = Grid.GetColumn(control);
                var focusNext =
                    _mainGrid.Children.OfType<CellControl>()
                    .FirstOrDefault(c => Grid.GetRow(control) == rowIndex + 1
                                            && Grid.GetColumn(control) == columnIndex)!;

                if(focusNext is not null)
                {
                    // to do
                    return;
                }
            }
        }
    }
}
