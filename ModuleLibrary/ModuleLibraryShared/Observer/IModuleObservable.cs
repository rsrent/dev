using System;
using System.Collections.Generic;

namespace ModuleLibraryShared.Observer
{
    public abstract class ModuleObservable<T>
    {
        private T model;

        public void SetModel(T model) {
            this.model = model;
        }

		public IDisposable Subscribe(IModuleObserver<T> observer)
		{
			if (!observers.Contains(observer))
				observers.Add(observer);
			return new Unsubscriber(observers, observer);
		}

		private class Unsubscriber : IDisposable
		{
			private List<IModuleObserver<T>> _observers;
			private IModuleObserver<T> _observer;

			public Unsubscriber(List<IModuleObserver<T>> observers, IModuleObserver<T> observer)
			{
				this._observers = observers;
				this._observer = observer;
			}

			public void Dispose()
			{
				if (_observer != null && _observers.Contains(_observer))
					_observers.Remove(_observer);
			}
		}

		List<IModuleObserver<T>> observers = new List<IModuleObserver<T>>();

		public void Update()
		{
			foreach (IModuleObserver<T> observer in observers) observer.ObservableUpdated(model);
		}
    }

    public interface IModuleObserver<T> {
        void ObservableUpdated(T model);
    }
}
