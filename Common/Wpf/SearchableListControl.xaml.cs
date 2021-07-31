using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Common.Wpf
{
    /// <summary>
    /// Interaction logic for SearchableListControl.xaml
    /// </summary>
    public partial class SearchableListControl : UserControl
    {
        public DependencyProperty DisplayTextProperty = DependencyProperty.Register(
            nameof(DisplayText), typeof(string), typeof(SearchableListControl), new PropertyMetadata());
        public string DisplayText
        {
            get => (string)GetValue(DisplayTextProperty);
            set => SetValue(DisplayTextProperty, value);
        }

        public DependencyProperty SearchableListItemSourceProperty = DependencyProperty.Register(
            nameof(SearchableListItemSource), typeof(ItemCollection), typeof(SearchableListControl), new PropertyMetadata());

        public ItemCollection SearchableListItemSource
        {
            get => (ItemCollection)GetValue(SearchableListItemSourceProperty);
            set => SetValue(SearchableListItemSourceProperty, value);
        }

        public ICommand DeleteItemCommand;

        public SearchableListControl()
        {
            InitializeComponent();
            throw new NotImplementedException();
        }

        public void DeleteItem()
        {

        }
    }
}
