using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using System.Linq;
using System.Reactive;

namespace Common.Avalonia.Control
{
    public partial class SheetControl : UserControl
    {
        private Grid _mainGrid;

        public StyledProperty<int> RowCountProperty = AvaloniaProperty.Register<SheetControl, int>(nameof(RowCount));
        public int RowCount
        {
            get => GetValue(RowCountProperty);
            set => SetValue(RowCountProperty, value);
        }

        public StyledProperty<int> ColumnCountProperty = AvaloniaProperty.Register<SheetControl, int>(nameof(ColumnCount));
        public int ColumnCount
        {
            get => GetValue(ColumnCountProperty);
            set => SetValue(ColumnCountProperty, value);
        }

        //public CellControl SelectedCellControl { get => _mainGrid}

        public SheetControl()
        {
            InitializeComponent();
            _mainGrid = this.FindControl<Grid>("MainGrid");

            int n = 100;
            int m = 10;

            for (int j = 0; j < m; j++) AddColumn();
            for (int i = 0; i < n; i++) AddRow();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void AddRow()
        {
            RowDefinition row = new RowDefinition();
            _mainGrid.RowDefinitions.Add(row);
            for (int i = 0; i < ColumnCount; i++)
            {
                CellControl cell = new CellControl();
                Grid.SetRow(cell, RowCount);
                Grid.SetColumn(cell, i);
                _mainGrid.Children.Add(cell);
            }
            RowCount += 1;
        }

        public void AddColumn()
        {
            ColumnDefinition column = new ColumnDefinition();
            _mainGrid.ColumnDefinitions.Add(column);
            for (int i = 0; i < RowCount; i++)
            {
                CellControl cell = new CellControl();
                Grid.SetColumn(cell, ColumnCount);
                Grid.SetRow(cell, i);
                _mainGrid.Children.Add(cell);
            }
            ColumnCount += 1;
        }

        public void DownRow()
        {
            foreach (CellControl control in _mainGrid.Children)
            {
                if (control.IsKeyboardFocusWithin)
                {
                    int rowIndex = Grid.GetRow(control);
                    int columnIndex = Grid.GetColumn(control);
                    CellControl focusNext =
                        _mainGrid.Children.OfType<CellControl>()
                        .FirstOrDefault(c => Grid.GetRow(control) == rowIndex + 1
                                                && Grid.GetColumn(control) == columnIndex);

                    if (focusNext is not null)
                    {
                        // to do
                        return;
                    }
                }
            }
        }
    }
}
