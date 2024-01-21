using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using System.Diagnostics;
using System.Linq;

namespace Eli.Avalonia.DataTemplateSelectors;

public class PropertyBindingTemplate : IDataTemplate
{
    private BuiltInTypeTemplate BuiltInTypeTemplate { get; } = new BuiltInTypeTemplate();

    public bool Match(object? data) => true;

    Control? ITemplate<object?, Control?>.Build(object? data)
    {
        if(data == null) return new TextBox { Text = data as string };
        if(BuiltInTypeTemplate.Match(data)) return ((ITemplate<object?, Control?>)BuiltInTypeTemplate).Build(data);

        var uniformGrid = new UniformGrid();
        foreach(var property in data.GetType().GetProperties().Where(p => p.SetMethod?.IsPublic == true))
        {
            Control control;
            var propertyValue = data.GetType().GetProperty(property.Name)?.GetValue(data);

            if(BuiltInTypeTemplate.Match(propertyValue)) control = ((ITemplate<object?, Control?>)BuiltInTypeTemplate).Build(propertyValue);
            else control = ((ITemplate<object?, Control?>)this).Build(propertyValue) ?? new TextBox { Text = data as string };

            if(control == null) throw new UnreachableException();

            control.DataContext = data;
            uniformGrid.Children.Add(control);
        }
        return uniformGrid;
    }
}
