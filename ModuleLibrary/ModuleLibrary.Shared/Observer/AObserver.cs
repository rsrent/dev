using System;
namespace ModuleLibrary.Shared.Observer
{
    public interface AObserver<T>
    {
        void Update(T model);
    }
}
