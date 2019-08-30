using System;
using ModuleLibrary.Shared.Storage;
using ModuleLibraryiOS.Storage;

namespace RentApp
{
    public class MyLocalStorageSettings : ILocalStorageSettings
    {
        public int CacheDurationInDays() => 7;
        public string StorageName() => "RentAppImageStorage";
    }
}
