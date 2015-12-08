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

            _logInCommand = new ObservableCommand();
            _cancelCommand = new ObservableCommand();

            _username.CombineLatest(_password, (username, password) => !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password)).Subscribe(_logInCommand);

            _logInCommand.Subscribe(ExecuteLogIn);
            _cancelCommand.Subscribe(ExecuteCancel);
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