using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BMS_Test_Client.Extensions;

namespace BMS_Test_Client
{
    public class LocationClient
    {
        private readonly ClientController _client;
        public LocationClient(ClientController client)
        {
            _client = client;
        }

        public async Task Run()
        {

            await Create();
            //var locations = await _client.Get("Location");
            //locations.Print();
        }

        public async Task Create()
        {
            var location = new
            {
                Name = "Pilestræde"
            };
            var result = await _client.PostId("Location/Create/2", location);
            Console.WriteLine("Result: " + result);
        }
    }
}
