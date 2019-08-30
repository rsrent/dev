using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace ModuleLibrary.Shared.Observer
{
    public class DataStore<K, M> where M : class
    {
        Dictionary<K, AObservable<M>> Store = new Dictionary<K, AObservable<M>>();

        IServiceProvider _serviceProvider;

        public DataStore(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public AObservable<M> TrySubscribe(AObserver<M> observer, K instanceID, M instance = null)
        {
            if (!Store.ContainsKey(instanceID) && instance != null)
            {
                var observable = _serviceProvider.GetService<AObservable<M>>();
                if(observable == null)
                    observable = new AObservable<M>();
                Store.Add(instanceID, observable);
                observable.Subscribe(observer);
                observable.Update(instance);
                return observable;
            }
            else if (Store.ContainsKey(instanceID) && instance != null)
            {
                var observable = Store[instanceID];
                observable.Subscribe(observer);
                observable.Update(instance);
                return observable;
            }
            else if (Store.ContainsKey(instanceID))
            {
                var observable = Store[instanceID];
                observable.Subscribe(observer);
                observable.Update();
                return observable;
            }
            return null;
        }

        public void Unsubscribe(AObserver<M> observer)
        {
            foreach (var t in Store.Values)
            {
                if (t.Observers.Contains(observer))
                    t.Unsubscribe(observer);
            }
        }

        public void RequestUpdate(AObserver<M> observer) 
        {
            foreach (var t in Store.Values)
            {
                if (t.Observers.Contains(observer))
                    t.Update();
            }
        }

        public void UpdateModel(K instanceID, Action<M> updateFunction)
        {
            if(Store.ContainsKey(instanceID))
            {
                updateFunction(Store[instanceID].Model);
                Store[instanceID].Update();
            }
        }
    }
}
