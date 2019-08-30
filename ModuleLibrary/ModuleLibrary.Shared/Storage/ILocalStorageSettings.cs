using System;
namespace ModuleLibrary.Shared.Storage
{
    public interface ILocalStorageSettings
    {
        string StorageName();
        int CacheDurationInDays();
    }
}
