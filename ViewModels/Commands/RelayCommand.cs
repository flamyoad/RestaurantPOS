using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlamyPOS.Commands
{
    // Generic version
    public class RelayCommand<T> : ICommand
    {
        Action<T> _execute;
        Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            if (execute == null)
                throw new NullReferenceException();

            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Action<T> execute) : this(execute, null)
        {

        }

        public bool CanExecute(object parameter)
        {
            return (_canExecute == null ? true : _canExecute((T)parameter));
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    // Non-generic
    public class RelayCommand : ICommand
    {
        Action _execute;
        Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new NullReferenceException();

            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Action execute) : this(execute, null)
        {

        }

        public bool CanExecute(object parameter)
        {
            return (_canExecute == null ? true : _canExecute());
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
