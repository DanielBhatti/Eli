using ReactiveUI;
using System;

namespace Eli.Avalonia.Mvvm;

public class Notifyable<T> : ReactiveObject
{
    public Type Type => Value is not null ? Value.GetType() : null!;
    private T _value = default!;
    public T Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }

    public Notifyable() => Value = default!;

    public Notifyable(T value) => Value = value;

    public Notifyable(object o) => Value = (T)o;

    public static implicit operator T(Notifyable<T> notifyable) => notifyable.Value;

    public static implicit operator Notifyable<T>(T value) => new(value);

    public override string ToString() => Value is not null ? Value.ToString()! : ""!;
}

public class Notifyable : Notifyable<object> { }
