using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
            typeof(ObservableCollection<FrameworkElement[]>),
            typeof(DynamicStackedGridControl),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnCollectionChanged)));
        public ObservableCollection<FrameworkElement[]> FrameworkElementCollection
        {
            get => (ObservableCollection<FrameworkElement[]>)GetValue(FrameworkElementCollectionProperty);
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

            grid.ColumnDefinitions.Add(new ColumnDefinition());
            Button button = new Button();
            button.Content = "Remove Row";
            button.Height = 30;
            button.Width = 200;
            button.Click += Button_RemoveStrategy;
            Grid.SetColumn(button, numColumns);
            Grid.SetRowSpan(button, numRows);
            grid.Children.Add(button);

            Children.Add(grid);
            return grid;
        }

        private void Button_RemoveStrategy(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Grid parentGrid = (Grid)button.Parent;

            RemoveGrid(parentGrid);
            FrameworkElement[] frameworkElements = FrameworkElementCollection.First(fes => fes[0].Parent == parentGrid);
            FrameworkElementCollection.Remove(frameworkElements);
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
            var action = new NotifyCollectionChangedEventHandler(
          (o, args) =>
          {
              var stack = d as DynamicStackedGridControl;

              if (stack != null)
              {
                  ObservableCollection<FrameworkElement[]> frameworkElements = (ObservableCollection<FrameworkElement[]>)e.NewValue;

                  foreach (FrameworkElement[] feArray in frameworkElements)
                  {
                      if (!stack.Children.Contains((Grid)feArray[0].Parent)) stack.AddGrid(1, feArray.Length, feArray);
                  }
              }
          });

            if (e.OldValue != null)
            {
                var coll = (INotifyCollectionChanged)e.OldValue;
                // Unsubscribe from CollectionChanged on the old collection
                coll.CollectionChanged -= action;
            }

            if (e.NewValue != null)
            {
                var coll = (ObservableCollection<FrameworkElement[]>)e.NewValue;
                // Subscribe to CollectionChanged on the new collection
                coll.CollectionChanged += action;
            }
        }
    }
}
