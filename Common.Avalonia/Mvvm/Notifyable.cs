using ReactiveUI;
using System;
using System.Runtime.CompilerServices;

namespace Common.Avalonia.Mvvm
{
    public class Notifyable<T> : ReactiveObject
    {
        public Type Type { get => Value is not null ? Value.GetType() : null!; }
        private T _value = default(T)!;
        public T Value
        {
            get => _value;
            set => this.RaiseAndSetIfChanged(ref _value, value);
        }

        public Notifyable() => Value = default(T)!;

        public Notifyable(T value) => Value = value;

        public Notifyable(object o) => Value = (T)o;

        public static implicit operator T(Notifyable<T> notifyable)
        {
            return notifyable.Value;
        }

        public static implicit operator Notifyable<T>(T value)
        {
            return new Notifyable<T>(value);
        }

        public override string ToString()
        {
            return Value is not null ? Value.ToString()! : ""!;
        }
    }

    public class Notifyable : Notifyable<object> { }
}
