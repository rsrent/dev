using System;
namespace Rent.Helpers
{
    public class MyAzureCredentials : AzureCredentials
    {
        public override string GetConnectionString()
        {
            return "DefaultEndpointsProtocol=https;AccountName=rentstorage;AccountKey=eLXdz46xRAG4XoqV7eUvJrL7jaMxkBfdfS6zvUCBBAJ5helhDZD8jlj85TLQ1T0c+ABKOkRV7PCl0UFNsSR6Lw==;EndpointSuffix=core.windows.net";
        }
    }
}
