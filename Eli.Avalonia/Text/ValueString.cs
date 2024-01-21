using System;
using System.Linq;

namespace Eli.Avalonia.Text;

public class ValueString : IValueContainer
{
    public string TextValue { get; private set; }
    public object? Value { get; private set; }
    public Type? Type { get; private set; }

    public ValueString(string textValue, Type type)
    {
        TextValue = textValue;
        SetType(type);
        SetValue(textValue);
    }

    public void SetValue(string textValue) => Update(textValue, Type);

    public void SetType(Type type) => Update(TextValue, type);

    public void Update(string textValue, Type? type)
    {
        TextValue = textValue;
        if(type is null)
        {
            return;
        }

        var parseMethod = type.GetMethod("Parse");
        if(parseMethod is not null
            && parseMethod.GetParameters().Count() == 1
            && parseMethod.GetParameters().First().ParameterType == typeof(string))
        {
            try
            {
                Value = parseMethod.Invoke(type, new object[] { textValue });
                Type = type;
            }
            catch
            {
                Value = null;
                Type = null;
            }
        }
        else if(type == typeof(string))
        {
            Value = textValue;
            Type = typeof(string);
        }
    }
}
