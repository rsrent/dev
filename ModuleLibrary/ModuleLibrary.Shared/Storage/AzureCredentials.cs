using System;
using System.Threading.Tasks;

namespace ModuleLibrary.Shared.Storage
{
    public abstract class AzureCredentials
    {
        public abstract Task<string> GetUploadConnectionString(string container);
        public abstract Task<string> GetDownloadConnectionString(string container);
    }
}
