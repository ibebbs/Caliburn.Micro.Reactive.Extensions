using System;
using System.Reactive.Linq;
using System.Windows.Input;

namespace Caliburn.Micro.Reactive.Extensions.Example
{
    public class ShellViewModel : PropertyChangedBase, IShell 
    {
        private ObservableProperty<string> _username;
        private ObservableProperty<string> _password;

        private ObservableCommand _logInCommand;
        private ObservableCommand _cancelCommand;

        public ShellViewModel()
        {
            _username = new ObservableProperty<string>(this, () => Username);
            _password = new ObservableProperty<string>(this, () => Password);

            _logInCommand = new ObservableCommand(CanLogIn);
            _cancelCommand = new ObservableCommand();

            _logInCommand.Subscribe(ExecuteLogIn);
            _cancelCommand.Subscribe(ExecuteCancel);
        }

        private IObservable<bool> CanLogIn
        {
            get { return _username.CombineLatest(_password, (username, password) => !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password)); }
        }

        private void ExecuteLogIn(object param)
        {
            // Perform log in
        }

        private void ExecuteCancel(object param)
        {
            // Cancel log in
        }

        public ICommand LogInCommand
        {
            get { return _logInCommand; }
        }

        public ICommand CancelCommand
        {
            get { return _cancelCommand; }
        }

        public string Username
        {
            get { return _username.Get(); }
            set { _username.Set(value); }
        }

        public string Password
        {
            get { return _password.Get(); }
            set { _password.Set(value); }
        }
    }
}