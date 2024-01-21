using ReactiveUI;
using System;

namespace Eli.Avalonia.ViewModels;

public class CheckBoxItemViewModel : ReactiveObject
{
    private string _name = "";
    public string Name
    {
        get => _name;
        set
        {
            _ = this.RaiseAndSetIfChanged(ref _name, value);
            PropertyChangedCallback(Name, IsChecked, Value);
        }
    }

    private bool _isChecked = false;
    public bool IsChecked
    {
        get => _isChecked;
        set
        {
            _ = this.RaiseAndSetIfChanged(ref _isChecked, value);
            PropertyChangedCallback(Name, IsChecked, Value);
        }
    }

    private object? _value = "";
    public object? Value
    {
        get => _value;
        set
        {
            _ = this.RaiseAndSetIfChanged(ref _value, value);
            PropertyChangedCallback(Name, IsChecked, Value);
        }
    }

    public Action<string, bool, object?> PropertyChangedCallback { get; set; }
    private Action<string, bool, object?> DoNothingAction { get; } = (s, b, o) => { };

    // PropertyChangedCallback must be set first
    // If not, it'll get called before it's set from RaiseAndSetIfChanged - when it's null, and throw a NullReferenceException
    public CheckBoxItemViewModel(string name, bool isChecked = true, object? value = null, Action<string, bool, object?> propertyChangedCallback = null!) => (PropertyChangedCallback, Name, IsChecked, Value) = (propertyChangedCallback ?? DoNothingAction, name, isChecked, value);
}
