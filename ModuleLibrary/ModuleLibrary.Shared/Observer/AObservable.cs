using System;
using System.Collections.Generic;

namespace ModuleLibrary.Shared.Observer
{
    public class AObservable<M>
    {
        public readonly List<AObserver<M>> Observers = new List<AObserver<M>>();

        M model;
        public M Model { get { return model; } set { model = value; } }

        public void Subscribe(AObserver<M> observer) {
            Observers.Add(observer);
        }

        public void Unsubscribe(AObserver<M> observer)
        {
            Observers.Remove(observer);
        }

        public void Update(M m)
        {
            Model = m;
            Update();
        }

        public void Update() {
            foreach (var observer in Observers)
                observer.Update(Model);
        }

        public void RequestUpdate(AObserver<M> observer)
        {
            observer.Update(Model);
        }
    }
}
