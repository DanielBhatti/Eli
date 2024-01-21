namespace Eli.Avalonia.Mvvm;

public interface IAlertable
{
    void Alert(string propertyName, string alertType = "", object? data = null);
}
