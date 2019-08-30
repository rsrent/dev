using System;
namespace RentApp
{
    public class Settings
    {
        public bool Live = true;

        //https://rentapp.azurewebsites.net
        string baseUri => Live ? "://rentapp.azurewebsites.net/" : "://rentapp-developmentstage.azurewebsites.net/";


        //string baseUri = "://localhost:5000/";


        public Uri ApiBaseAddress => new Uri("https"+baseUri+"api/");
        //public Uri ApiBaseAddress => new Uri("http://rent20170925043224.azurewebsites.net/api/");
        //public Uri ApiBaseAddress => new Uri("http://localhost:5000/api/");

        //public string HttpUri => "ws://rent20170925043224.azurewebsites.net/chat";
        //public string SocketUri => "ws" + baseUri + "chat/";
        public string SocketUri => "wss" + baseUri + "chat/";
        //public string SocketUri => "http" + baseUri + "chat";
		//public string AzureStorageConnectionString => "DefaultEndpointsProtocol=https;AccountName=rentappteststorage;AccountKey=vdKiDLv4xqGWVFRQEhkCSIYWeb1kJIdvlTj5fNifYt5DUiT2BGYX7lI2cV0gwcaNReDCDClFbOoZxgUBaj+h6A==;EndpointSuffix=core.windows.net";
    }
}