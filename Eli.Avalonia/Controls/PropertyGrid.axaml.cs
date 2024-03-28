using Avalonia;
using Avalonia.Controls;
using System.Reflection;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using System.Linq;

namespace Eli.Avalonia.Controls;

public partial class PropertyGrid : UserControl
{
    public static readonly DirectProperty<PropertyGrid, object?> ObjectProperty = AvaloniaProperty.RegisterDirect<PropertyGrid, object?>(
        nameof(Object),
        o => o.Object,
        (o, v) => o.Object = v);

    private object? _object;
    public object? Object
    {
        get => _object;
        set
        {
            _ = SetAndRaise(ObjectProperty, ref _object, value);
            LoadObject();
        }
    }

    public PropertyGrid() => InitializeComponent();

    private void LoadObject()
    {
        if(_object is null) return;

        var panel = Content as UniformGrid ?? new UniformGrid { Columns = 2 };
        Content = panel;
        panel.Children.Clear();

        var objectType = _object.GetType();
        var properties = objectType.GetProperties().OrderBy(p => p.Name);

        foreach(var property in properties)
        {
            var control = CreateControlForProperty(property);
            if(control != null)
            {
                panel.Children.Add(control);
            }
        }
    }

    private Control? CreateControlForProperty(PropertyInfo propertyInfo)
    {
        var container = new UniformGrid { Columns = 2 };

        var textBlock = new TextBlock { Text = propertyInfo.Name };
        container.Children.Add(textBlock);

        var textBox = new TextBox();

        if(_object != null)
        {
            var binding = new Binding
            {
                Source = _object,
                Path = propertyInfo.Name,
                Mode = BindingMode.TwoWay,
            };
            _ = textBox.Bind(TextBox.TextProperty, binding);
        }
        container.Children.Add(textBox);

        return container;
    }
}
