using System.Windows;
using System.Windows.Input;

namespace MouseJiggler
{
    class ShutdownCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            Application.Current.Shutdown();
        }
    }
}
