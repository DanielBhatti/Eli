using Avalonia.Controls;
using Avalonia.Input;
using System;

namespace Eli.Avalonia.Controls;

public partial class TabNavigatableControl : TabControl
{
    public TabNavigatableControl()
    {
        InitializeComponent();
    }

    // see https://github.com/AvaloniaUI/Avalonia/issues/12967
    protected override Type StyleKeyOverride => typeof(TabControl);

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if(e.Key == Key.Tab && e.KeyModifiers == KeyModifiers.Control)
        {
            ChangeTab(1);
            e.Handled = true;
        }
        else if(e.Key == Key.Tab && e.KeyModifiers == (KeyModifiers.Control | KeyModifiers.Shift))
        {
            ChangeTab(-1);
            e.Handled = true;
        }
    }

    // adding Items.Count is a workaround for the modulo operator return negative numbers 
    private void ChangeTab(int direction) => SelectedIndex = (SelectedIndex + direction + Items.Count) % Items.Count;
}
