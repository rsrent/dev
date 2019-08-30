using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.Helpers
{
    public class AzureDevCredentials : AzureCredentials
    {
        private readonly string DevelopmentConnectionString = "DefaultEndpointsProtocol=https;AccountName=rentdevelopmentstorage;AccountKey=9+6x4oSeFRT8BC3qFDEpHLgRky3CNV8b+Tt41J5XONeQNM+M4/GFHbYB9fdNh1tGicAyh1aBYXWyZuXcMV4iHg==;EndpointSuffix=core.windows.net";         public override string GetConnectionString()         {             return DevelopmentConnectionString;
        }
    }
}
