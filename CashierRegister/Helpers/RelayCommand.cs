using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CashierRegister.Helpers
{
    public class RelayCommand : ICommand
    {
        #region Fields

        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        #endregion // Fields

        #region Constructors

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion // Constructors

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion // ICommand Members
    }
    public class RelayCommand<T> : ICommand
    {
        readonly Predicate<T> _predicate = null;
        readonly Action<T> _action = null;

        public RelayCommand(Action<T> action)
            : this(null, action)
        {
        }

        public RelayCommand(Predicate<T> predicate, Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            _predicate = predicate;
            _action = action;
        }
        public bool CanExecute(object parameter)
        {
            return _predicate == null ? true : _predicate((T)parameter);
        }
        public void Execute(object parameter)
        {
            _action((T)parameter);
        }
        //public event EventHandler CanExecuteChanged;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        //public void RaiseCanExecuteChanged()
        //{
        //    CanExecuteChanged(this, EventArgs.Empty);
        //}
    }
}
