Caliburn.Micro.Reactive.Extensions
==================================

### Rx based extensions for Caliburn Micro - [![cogenity MyGet Build Status](https://www.myget.org/BuildSource/Badge/cogenity?identifier=ca24b530-5018-4566-8d57-05b62ef15b08)](https://www.myget.org/)

A few constructs that allow the developer to write less code, in a more declarative manner resulting in less bugs and better performance. The classes are similar in nature to those found in ReactiveUI but are simpler (relying on the developer composing higher-order functionality rather than trying to build it in) and tied to Caliburn Micro (mostly as it is my MVVM framework of choice and provides a convenient extension interfaces - namely INotifyPropertyChangedEx).

## Example

The following view model demonstrates the use of ObservableProperty and ObservableCommand classes by implementing a typical 'log in' dialog. In the dialog, the user can cancel the log in process at any time, but can only attempt to log in once a username and password have been entered.

```c#
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

            _logInCommand = new ObservableCommand(CanLogIn, ExecuteLogIn);
            _cancelCommand = new ObservableCommand(ExecuteCancel);
        }

        private IObservable<bool> CanLogIn
        {
            get 
            {
                return _username.Values.CombineLatest(
                    _password.Values, 
                    (username, password) => !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password)
                ); 
            }
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
```

## Classes

### ObservableProperty

The ObservableProperty class is  much like a dependency property. It is declared in place of a backing field, starts with a specific default value, has get and set methods to change the current value, is typically exposed as a property and automatically handles property change notifications. On top of this, ObservableProperty instances are generic (removing the awful casting you need with DependencyProperty getters) and expose IObservable<T> of values which can be used to compose functionality in the UI.

### ObservableCommand

The ObservableCommand class implements ICommand and can therefore be bound directly to controls in the UI. It takes an IObservable<bool> as a constructor parameter which it uses to determine whether the command is currently enabled or not (no need for nasty 'NotifyCanxecuteHasChanged' methods). It exposes an IObservable<object> that emits a value (the command parameter value) when the command is executed. Alternatively, an Action<object> can be specified in the constructor which will also be called whenever the command is executed.

### ObservableEvents

ObservableEvents is a collection of extension methods for exposing Caliburn / WPF events as observable sequences. For example 'FromPropertyChanged' maps the PropertyChangedEvent from any class implementing INotifyPropertyChanged to an IObservable<PropertyChangedEventArgs>'.

## Todo

* Tests!
* Exposing Conductor / Screen events as Observable sequences

## Packages And Symbols

Nuget package available from [our nuget feed](https://www.myget.org/F/cogenity/)

Symbols available from [SymbolSource](http://srv.symbolsource.org/pdb/MyGet/ibebbs/d58332dc-dffe-4da8-ba6a-8b1a73b759e9)
