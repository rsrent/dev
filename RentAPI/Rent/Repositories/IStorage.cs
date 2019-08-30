using System.IO;
using System.Threading.Tasks;

namespace Rent.Repositories
{
    public interface IStorage
    {
        Task<string> UploadImage(string filePath);
        Task<string> UploadImage(Stream stream);
        Task<string> Upload(string containerName, string filePath);
        Task<byte[]> GetImage(string location);
        Task<bool> CreateContainer(string name);
        Task<byte[]> Get(string containerName, string location);
        Task<bool> DeleteContainerAsync(string containerName);
        Task<string> GetSASTokenUpload(string containerName);
        string GetBlobSasUri(string containerName, string blobName, string policyName);
        Task<string> GetContainerSas(string containerName, bool delete = false);
    }
}
