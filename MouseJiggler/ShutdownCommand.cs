using System.Windows;
using System.Windows.Input;

namespace MouseJiggler;

class ShutdownCommand : ICommand
{
#pragma warning disable 0067
    public event EventHandler? CanExecuteChanged;
#pragma warning restore 0067

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        Application.Current.Shutdown();
    }
}