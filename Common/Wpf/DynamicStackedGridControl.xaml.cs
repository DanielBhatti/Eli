using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Common.Wpf
{
    /// <summary>
    /// Interaction logic for DynamicStackedGridControl.xaml
    /// </summary>
    public partial class DynamicStackedGridControl : StackPanel
    {
        public static readonly DependencyProperty FrameworkElementCollectionProperty = DependencyProperty.Register(
            nameof(FrameworkElementCollection),
            typeof(ObservableCollection<FrameworkElement>),
            typeof(DynamicStackedGridControl),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnCollectionChanged)));
        public ObservableCollection<FrameworkElement> FrameworkElementCollection
        {
            get => (ObservableCollection<FrameworkElement>)GetValue(FrameworkElementCollectionProperty);
            set => SetValue(FrameworkElementCollectionProperty, value);
        }

        public DynamicStackedGridControl()
        {
            InitializeComponent();
        }

        public Grid AddGrid(int numRows, int numColumns, params FrameworkElement[] frameworkElements)
        {
            if (numRows < 1 || numColumns < 1)
                throw new ArgumentOutOfRangeException($"Number of rows and columns must be positive integers, provided values were rows: {numRows} and columns: {numColumns}.");
            if (frameworkElements.Count() != numRows * numColumns)
                throw new ArgumentException($"Number of FrameworkElements ({frameworkElements.Count()}) was not equal to the Number of Rows * Columns ({numRows * numColumns}).");

            Grid grid = new Grid();
            for (int i = 0; i < numColumns; i++)
            {
                for (int j = 0; j < numRows; j++)
                {
                    FrameworkElement fe = frameworkElements[i];
                    ColumnDefinition columnDefinition = new ColumnDefinition();
                    RowDefinition rowDefinition = new RowDefinition();
                    grid.ColumnDefinitions.Add(columnDefinition);
                    grid.RowDefinitions.Add(rowDefinition);
                    grid.Children.Add(fe);
                    Grid.SetColumn(fe, i);
                    Grid.SetRow(fe, j);
                }
            }
            Children.Add(grid);
            return grid;
        }

        public void RemoveGrid(Grid grid)
        {
            Children.Remove(grid);
        }

        public List<Grid> GetGrids()
        {
            List<Grid> grids = new List<Grid>() { };
            foreach (Grid grid in Children.OfType<Grid>())
            {
                grids.Add(grid);
            }
            return grids;
        }

        private static void OnCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
