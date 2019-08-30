using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BMS_Test_Client.Extensions;

namespace BMS_Test_Client
{
    public class CleaningTaskClient
    {
        private readonly ClientController _client;
        public CleaningTaskClient(ClientController client)
        {
            _client = client;
        }

        public async Task Run()
        {
            var contetnt = new
            {
                squareMeters = 555,
                frequency = "55",
                comment = "55",
                description = "Hey",
                place = "Here",
                active = true,
            };
            var result = await _client.PostNoContent("CleaningTask/Create/1", contetnt);
            Console.WriteLine("Result: " + result);
        }
    }
}
