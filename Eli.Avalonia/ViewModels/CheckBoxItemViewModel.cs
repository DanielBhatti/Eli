using ReactiveUI;
using System;

namespace Eli.Avalonia.ViewModels;

public class CheckBoxItemViewModel : ReactiveObject
{
    public string Name
    {
        get;
        set
        {
            _ = this.RaiseAndSetIfChanged(ref field, value);
            PropertyChangedCallback(Name, IsChecked, Value);
        }
    } = "";

    public bool IsChecked
    {
        get;
        set
        {
            _ = this.RaiseAndSetIfChanged(ref field, value);
            PropertyChangedCallback(Name, IsChecked, Value);
        }
    } = false;

    public object? Value
    {
        get;
        set
        {
            _ = this.RaiseAndSetIfChanged(ref field, value);
            PropertyChangedCallback(Name, IsChecked, Value);
        }
    } = "";

    public Action<string, bool, object?> PropertyChangedCallback { get; set; }
    private Action<string, bool, object?> DoNothingAction { get; } = (s, b, o) => { };

    // PropertyChangedCallback must be set first
    // If not, it'll get called before it's set from RaiseAndSetIfChanged - when it's null, and throw a NullReferenceException
    public CheckBoxItemViewModel(string name, bool isChecked = true, object? value = null, Action<string, bool, object?> propertyChangedCallback = null!) => (PropertyChangedCallback, Name, IsChecked, Value) = (propertyChangedCallback ?? DoNothingAction, name, isChecked, value);
}
