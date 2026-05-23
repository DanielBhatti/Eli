using ReactiveUI;
using System.Diagnostics.CodeAnalysis;

namespace Eli.Avalonia.ViewModels;

public class NamedObjectViewModel<T> : ReactiveObject
{
    public required string Name
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = "";
    public required string Description
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = "";
    public required T Value
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = default!;

    public NamedObjectViewModel() { }

    [SetsRequiredMembers]
    public NamedObjectViewModel(string name, string description, T value) => (Name, Description, Value) = (name, description, value);
}
