using System;
using System.Threading.Tasks;
using BMS_Test_Client.Extensions;

namespace BMS_Test_Client
{
    public class CommentClient
    {
        private readonly ClientController _client;
        public CommentClient(ClientController client)
        {
            _client = client;
        }

        public async Task Run()
        {
            await Get(1036);
        }



        public async Task Get(int commentId)
        {
            var result = await _client.Get("Comment/Get/" + commentId);
            result.Print();
        }


    }
}
