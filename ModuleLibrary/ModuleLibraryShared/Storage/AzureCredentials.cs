using System;
using System.Threading.Tasks;

namespace ModuleLibraryShared.Storage
{
    public abstract class AzureCredentials
    {
        public abstract Task<string> GetUploadConnectionString(string container);
        public abstract Task<string> GetDownloadConnectionString(string container);
    }
}
