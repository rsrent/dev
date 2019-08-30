using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BMS_Test_Client.Extensions;

namespace BMS_Test_Client
{
    public class InvitationClient
    {
        private readonly ClientController _client;
        public InvitationClient(ClientController client)
        {
            _client = client;
        }

        public async Task Run()
        {
            //await CreateInvitation();
            //await ReplyNoToInvite();
        }

        public async Task CreateInvitation()
        {
            var res = await _client.PostNoContent("WorkInvitation/Create/109/3");
            Console.WriteLine(res);
        }
        public async Task ReplyNoToInvite()
        {
            var res = await _client.PutNoContent("WorkInvitation/Update/109/true");
            Console.WriteLine(res);
        }
    }
}
