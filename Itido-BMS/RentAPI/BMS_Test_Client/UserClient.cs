using System;
using System.Threading.Tasks;
using BMS_Test_Client.Extensions;
using System.Collections.Generic;

namespace BMS_Test_Client
{
    public class UserClient
    {
        private readonly ClientController _client;
        public UserClient(ClientController client)
        {
            _client = client;
        }

        public async Task Run()
        {
            await GetInvited();
        }

        public async Task GetInvited()
        {
            var res = await _client.GetMany("Users/GetInvitedUsers/132");
            res.Print();
        }
    }
}