using System.Windows;
using System.Windows.Input;

namespace MouseJiggler
{
    class StartStopCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            App app = (App)Application.Current;
            app.JiggleActive = !app.JiggleActive;
        }
    }
}
