using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

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

        public SheetControl()
        {
            InitializeComponent();
            _mainGrid = this.FindControl<Grid>("MainGrid");

            int n = 10;
            int m = 10;

            for (int i = 0; i < n; i++) AddRow();
            for (int j = 0; j < m; j++) AddColumn();
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
    }
}
