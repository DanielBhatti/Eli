using System;
using System.Windows.Input;

namespace Eli.Test;

public abstract class Command : ICommand
{
    public abstract event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter) => true;

    public abstract void Execute(object parameter);
}
