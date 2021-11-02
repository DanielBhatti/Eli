using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using ReactiveUI;

namespace Common.Avalonia.Mvvm
{
    public class Notifyable<T> : ReactiveObject
    {
        private T _value = default(T);
        public T Value
        {
            get => _value;
            set => this.RaiseAndSetIfChanged(ref _value, value);
        }

        public static implicit operator T(Notifyable<T> notifyable)
        {
            return notifyable.Value;
        }
    }
}
