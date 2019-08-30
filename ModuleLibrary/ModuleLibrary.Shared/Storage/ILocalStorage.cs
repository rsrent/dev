using System;
namespace ModuleLibrary.Shared.Storage
{
    public interface ILocalStorage<T> where T : class
    {
        void Put(string key, T value);
        T Get(string key);
    }
}
