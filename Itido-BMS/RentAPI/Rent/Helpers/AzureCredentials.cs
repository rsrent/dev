using System;
namespace Rent.Helpers
{
    public abstract class AzureCredentials     {         public string ConnectionString => GetConnectionString();          public abstract string GetConnectionString();     }
}
