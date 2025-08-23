using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;

namespace Eli.Avalonia.Controls;

public partial class CalendarDateOnlyPicker : CalendarDatePicker
{
    protected override Type StyleKeyOverride => typeof(CalendarDatePicker);

    public static readonly DirectProperty<CalendarDateOnlyPicker, DateOnly?> SelectedDateOnlyProperty =
        AvaloniaProperty.RegisterDirect<CalendarDateOnlyPicker, DateOnly?>(
            nameof(SelectedDateOnly),
            o => o.SelectedDateOnly,
            (o, v) => o.SelectedDateOnly = v,
            defaultBindingMode: BindingMode.TwoWay);

    private DateOnly? _selectedDateOnly;
    private bool _sync;

    public DateOnly? SelectedDateOnly
    {
        get => _selectedDateOnly;
        set
        {
            if(_sync) return;
            if(!SetAndRaise(SelectedDateOnlyProperty, ref _selectedDateOnly, value)) return;

            try
            {
                _sync = true;
                base.SelectedDate = value.HasValue
                    ? DateTime.SpecifyKind(new DateTime(value.Value.Year, value.Value.Month, value.Value.Day), DateTimeKind.Unspecified)
                    : (DateTime?)null;
            }
            finally { _sync = false; }
        }
    }

    public CalendarDateOnlyPicker()
    {
        if(base.SelectedDate is DateTime init)
            _selectedDateOnly = new DateOnly(init.Year, init.Month, init.Day);

        SelectedDateChanged += (_, __) =>
        {
            if(_sync) return;
            try
            {
                _sync = true;
                if(base.SelectedDate is DateTime dt)
                    _ = SetAndRaise(SelectedDateOnlyProperty, ref _selectedDateOnly, new DateOnly(dt.Year, dt.Month, dt.Day));
                else
                    _ = SetAndRaise(SelectedDateOnlyProperty, ref _selectedDateOnly, null);
            }
            finally { _sync = false; }
        };
    }
}
