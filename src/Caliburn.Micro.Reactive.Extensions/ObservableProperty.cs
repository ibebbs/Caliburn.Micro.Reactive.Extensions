using System;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Caliburn.Micro.Reactive.Extensions
{
    public class ObservableProperty<TValue> : IObservable<TValue>, IObserver<TValue>
    {
        private TValue _current;
        private BehaviorSubject<TValue> _values;
        private IDisposable _notifySubscription;

        public ObservableProperty(TValue defaultValue, INotifyPropertyChangedEx notifier, string propertyName)
        {
            _current = defaultValue;
            _values = new BehaviorSubject<TValue>(defaultValue);

            _notifySubscription = _values.DistinctUntilChanged().Do(value => _current = value).Subscribe(value => notifier.NotifyOfPropertyChange(propertyName));
        }

        public ObservableProperty(INotifyPropertyChangedEx notifier, string propertyName) : this(default(TValue), notifier, propertyName) { }

        public ObservableProperty(TValue defaultValue, INotifyPropertyChangedEx notifier, Expression<Func<TValue>> property) : this(defaultValue, notifier, property.GetMemberInfo().Name) { }

        public ObservableProperty(INotifyPropertyChangedEx notifier, Expression<Func<TValue>> property) : this(default(TValue), notifier, property) { }

        IDisposable IObservable<TValue>.Subscribe(IObserver<TValue> observer)
        {
            return _values.Subscribe(observer);
        }

        void IObserver<TValue>.OnCompleted()
        {
            // Do nothing
        }

        void IObserver<TValue>.OnError(Exception error)
        {
            // Do nothing
        }

        void IObserver<TValue>.OnNext(TValue value)
        {
            _values.OnNext(value);
        }

        public TValue Get()
        {
            return _current;
        }

        public void Set(TValue value)
        {
            _values.OnNext(value);
        }
    }
}
