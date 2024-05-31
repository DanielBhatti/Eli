using ReactiveUI;
using System.Diagnostics.CodeAnalysis;

namespace Eli.Avalonia.ViewModels;

public class NamedObjectViewModel<T> : ReactiveObject
{
    private string _name = "";
    public required string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private string _description = "";
    public required string Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    private T _value = default!;
    public required T Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }

    public NamedObjectViewModel() { }

    [SetsRequiredMembers]
    public NamedObjectViewModel(string name, string description, T value) => (Name, Description, Value) = (name, description, value);
}
