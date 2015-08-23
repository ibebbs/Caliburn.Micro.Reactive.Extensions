using System;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Caliburn.Micro.Reactive.Extensions
{
    public class ObservableProperty<T> : ISubject<T>, IDisposable
    {
        private readonly Subject<T> _userChanges;
        private readonly Subject<T> _sourceChanges;

        private readonly IObservable<T> _values;

        private IDisposable _propertyChanges;
        private T _current;

        public ObservableProperty(INotifyPropertyChangedEx propertyNotifier, T defaultValue, string propertyName)
        {
            _userChanges = new Subject<T>();
            _sourceChanges = new Subject<T>();

            _values = _sourceChanges.Merge(_userChanges).StartWith(defaultValue).Do(value => _current = value).DistinctUntilChanged().Replay(1).RefCount();
            _propertyChanges = _values.Skip(1).Subscribe(pc => propertyNotifier.NotifyOfPropertyChange(propertyName));

            UserChanges = _userChanges.DistinctUntilChanged();
        }

        public ObservableProperty(INotifyPropertyChangedEx notifier, T defaultValue, Expression<Func<T>> property) : this(notifier, defaultValue, property.GetMemberInfo().Name) { }

        public ObservableProperty(INotifyPropertyChangedEx notifier, Expression<Func<T>> property) : this(notifier, default(T), property) { }

        public void Dispose()
        {
            if (_propertyChanges != null)
            {
                _propertyChanges.Dispose();
                _propertyChanges = null;
            }
        }

        public IObservable<T> UserChanges { get; private set; }

        void IObserver<T>.OnCompleted()
        {
            // Do nothing
        }

        void IObserver<T>.OnError(Exception error)
        {
            // Do nothing
        }

        void IObserver<T>.OnNext(T value)
        {
            _sourceChanges.OnNext(value);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _values.Subscribe(observer);
        }

        public T Get()
        {
            return _current;
        }

        public void Set(T value)
        {
            _userChanges.OnNext(value);
        }
    }
}
