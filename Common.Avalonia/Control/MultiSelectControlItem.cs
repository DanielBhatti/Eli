using Avalonia;
using ReactiveUI;
using System.Reactive;

namespace Common.Avalonia.Control
{
    public class MultiSelectControlItem : AvaloniaObject
    {
        public static StyledProperty<string> TextProperty = AvaloniaProperty.Register<CellControl, string>(nameof(Text));
        public string Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static StyledProperty<bool> IsCheckedProperty = AvaloniaProperty.Register<CellControl, bool>(nameof(IsChecked));
        public bool IsChecked
        {
            get => GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public ReactiveCommand<Unit, Unit> CheckedCommand { get; set; } = ReactiveCommand.Create(() => { return; });
        public ReactiveCommand<Unit, Unit> UncheckedCommand { get; set; } = ReactiveCommand.Create(() => { return; });
    }
}
