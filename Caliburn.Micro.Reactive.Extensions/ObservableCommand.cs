using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;

namespace Caliburn.Micro.Reactive.Extensions
{
    public class ObservableCommand : ICommand, IDisposable
    {
        private Subject<object> _invocations;

        private bool _canExecute;
        private IDisposable _canExecuteSubscription;

        public event EventHandler CanExecuteChanged;

        public ObservableCommand(IObservable<bool> canExecute)
        {
            _invocations = new Subject<object>();
            _canExecuteSubscription = canExecute.DistinctUntilChanged().Subscribe(OnCanExecuteChanged);
        }

        public ObservableCommand(IObservable<bool> canExecute, Action<object> execute) : this(canExecute)
        {
            _invocations.Subscribe(execute);
        }

        public ObservableCommand() : this(Observable.Return<bool>(true)) { }

        public ObservableCommand(Action<object> execute) : this(Observable.Return<bool>(true), execute) { }

        public void Dispose()
        {
            if (_canExecuteSubscription != null)
            {
                _canExecuteSubscription.Dispose();
                _canExecuteSubscription = null;
            }

            if (_invocations != null)
            {
                _invocations.Dispose();
                _invocations = null;
            }
        }

        private void OnCanExecuteChanged(bool canExecute)
        {
            _canExecute = canExecute;

            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            _invocations.OnNext(parameter);
        }

        public IObservable<object> Invocations
        {
            get { return _invocations; }
        }
    }
}
