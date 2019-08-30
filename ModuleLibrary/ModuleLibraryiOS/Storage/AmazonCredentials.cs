using System;
namespace ModuleLibraryiOS.Storage
{
    public class AmazonCredentials
    {
        public string AccessKeyId { get; set; }
        public string SecretAccessKey { get; set; }
        public string SessionToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
