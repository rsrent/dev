using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BMS_Test_Client.Extensions;

namespace BMS_Test_Client
{

    public class WorkRegistrationClient
    {

        private readonly ClientController _client;

        public WorkRegistrationClient(ClientController client)
        {
            _client = client;
        }

        public async Task Run()
        {
            await RegisterWork();
            //await ReplyToRegistration();
        }

        public async Task RegisterWork()
        {
            var res = await _client.PostId("Work/RegisterWork/77/480/720");
            Console.WriteLine(res);
        }

        public async Task ReplyToRegistration()
        {
            var res = await _client.PutNoContent("Work/ReplyToRegistration/15/false");
            Console.WriteLine(res);

        }

    }
}
