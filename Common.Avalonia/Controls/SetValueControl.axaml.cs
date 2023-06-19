using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using System;
using Common.Avalonia.Converters;
using Avalonia;

namespace Common.Avalonia.Controls
{
    public class SetValueControl : UserControl
    {
        public static readonly StyledProperty<object> ValueProperty = AvaloniaProperty.Register<SetValueControl, object>(nameof(Value));
        private ContentPresenter? contentPresenter;
        public SetValueControl() => InitializeComponent();

        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            this.GetObservable(ValueProperty).Subscribe(_ => UpdateControl());
        }

        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> e)
        {
            base.OnPropertyChanged(e);
            if(e.Property == ValueProperty) UpdateControl();
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            contentPresenter = e.NameScope.Find<ContentPresenter>("PART_ContentPresenter");
            UpdateControl();
        }

        private void UpdateControl()
        {
            contentPresenter = this.Find<ContentPresenter>("PART_ContentPresenter");
            contentPresenter.Bind(ContentPresenter.ContentProperty, new Binding("Value") { Source = this });

            if(contentPresenter is not null)
            {
                if(Value is string s)
                {
                    var textBox = new TextBox();
                    textBox.Bind(TextBox.TextProperty, new Binding("Value") { Source = this });
                    contentPresenter.Content = textBox;
                }
                else if(Value is DateTime dt)
                {
                    var datePicker = new DatePicker();
                    datePicker.Bind(DatePicker.SelectedDateProperty, new Binding("Value") { Source = this });
                    contentPresenter.Content = datePicker;
                }
                else if(Value is bool b)
                {
                    var checkBox = new CheckBox();
                    checkBox.Bind(CheckBox.IsCheckedProperty, new Binding("Value") { Source = this });
                    contentPresenter.Content = checkBox;
                }
                else if(Value is char c)
                {
                    var textBox = new TextBox();
                    textBox.Bind(TextBox.TextProperty, new Binding("Value") { Source = this, Converter = new CharToStringConverter() });
                    contentPresenter.Content = textBox;
                }
                else if(Value is int || Value is byte || Value is short || Value is ushort || Value is uint || Value is long || Value is ulong || Value is float || Value is double || Value is decimal)
                {
                    var numericUpDown = new NumericUpDown();
                    numericUpDown.Bind(NumericUpDown.ValueProperty, new Binding("Value") { Source = this, Converter = new NumericToDoubleConverter() });
                    contentPresenter.Content = numericUpDown;
                }
                else if(Value is Enum enumValue)
                {
                    var comboBox = new ComboBox();
                    comboBox.Items = Enum.GetValues(enumValue.GetType());
                    comboBox.SelectedItem = enumValue;
                    comboBox.Bind(ComboBox.SelectedItemProperty, new Binding("Value") { Source = this });
                    contentPresenter.Content = comboBox;
                }
                else
                {
                    var textBox = new TextBox();
                    textBox.Bind(TextBox.TextProperty, new Binding("Value") { Source = this });
                    contentPresenter.Content = textBox;
                }
            }
        }
    }
}
