using System;

namespace Common.Avalonia.Text
{
    public interface IValueContainer
    {
        string TextValue { get; }
        object? Value { get; }
        Type? Type { get; }

        void SetValue(string value);

        void SetType(Type type);
    }
}
