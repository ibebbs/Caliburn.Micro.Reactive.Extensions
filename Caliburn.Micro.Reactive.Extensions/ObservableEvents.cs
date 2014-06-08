using System;
using System.ComponentModel;
using System.Reactive.Linq;

namespace Caliburn.Micro.Reactive.Extensions
{
    public static class ObservableEvents
    {
        public static IObservable<PropertyChangedEventArgs> FromPropertyChanged(this INotifyPropertyChanged source)
        {
            return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(handler => source.PropertyChanged += handler, handler => source.PropertyChanged -= handler).Select(handler => handler.EventArgs);
        }
    }
}
