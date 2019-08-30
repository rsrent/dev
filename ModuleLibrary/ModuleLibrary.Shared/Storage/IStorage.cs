using System;
using System.IO;
using System.Threading.Tasks;

namespace ModuleLibrary.Shared.Storage
{
    public interface IStorage
    {
        Task<string> Upload(string blobPath, string container);
        Task<string> Upload(Stream blobStream, string container, string name = null);
        Task<byte[]> Download(string name, string container);
    }
}
