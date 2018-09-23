using System;
using System.Windows.Input;

namespace WifiKey.ViewModel
{

    /// <inheritdoc />
    /// <summary>
    /// Class RelayCommand.
    /// </summary>
    public class RelayCommand<T> : RelayCommand
    {
        /// <summary>
        /// The _execute
        /// </summary>

        private readonly Action<T> execute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        /// <exception cref="System.ArgumentNullException">execute</exception>
        public RelayCommand(Action<T> execute, Func<bool> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));

            if (canExecute != null)
                this.canExecute = canExecute;
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the RelayCommand class that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <exception cref="T:System.ArgumentNullException">If the execute argument is null.</exception>
        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public override void Execute(object parameter)
        {
            if (CanExecute(parameter))
                execute.Invoke((T)parameter);
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// Class RelayCommand.
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// The _execute
        /// </summary>
        private readonly Action execute;

        /// <summary>
        /// The _can execute
        /// </summary>
        protected Func<bool> canExecute;

        protected RelayCommand()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        /// <exception cref="System.ArgumentNullException">execute</exception>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));

            if (canExecute != null)
                this.canExecute = canExecute;
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the RelayCommand class that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <exception cref="T:System.ArgumentNullException">If the execute argument is null.</exception>
        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;


        /// <summary>
        /// Raises the can execute changed.
        /// </summary>
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        /// <inheritdoc />
        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter) => canExecute == null || canExecute.Invoke();

        /// <inheritdoc />
        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public virtual void Execute(object parameter)
        {
            if (CanExecute(parameter))
                execute.Invoke();
        }
    }
}
