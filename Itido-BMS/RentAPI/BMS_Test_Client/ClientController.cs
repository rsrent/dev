using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BMS_Test_Client.Extensions;

namespace BMS_Test_Client
{
    public class ClientController
    {
        private HttpClient _client;
        private string _basePath;

        Uri Path(string end)
        {
            var _path = _basePath + end;
            Console.WriteLine("\nPath: " + _path);
            return new Uri(_path);
        }
        public ClientController(string basePath)
        {
            _basePath = basePath;
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task Login(string userName, string password)
        {
            var content = new { UserName = userName, Password = password };
            var logs = await _client.Post<Dictionary<string, object>>(Path("Logins/Login/"), content: content);
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + logs["token"]);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GetString(string path)
        {
            return await _client.Get<string>(Path(path));
        }

        public async Task<Dictionary<string, object>> Get(string path)
        {
            return await _client.Get<Dictionary<string, object>>(Path(path));
        }

        public async Task<ICollection<Dictionary<string, object>>> GetMany(string path)
        {
            return await _client.Get<ICollection<Dictionary<string, object>>>(Path(path));
        }

        public async Task<int> PostId(string path, object content = null)
        {
            return await _client.Post<int>(Path(path), content);
        }

        public async Task<int> PutId(string path, object content = null)
        {
            return await _client.Put<int>(Path(path), content);
        }

        public async Task<bool> PostNoContent(string path, object content = null)
        {
            return await _client.PostNoContent(Path(path), content);
        }

        public async Task<bool> PutNoContent(string path, object content = null)
        {
            return await _client.PutNoContent(Path(path), content);
        }
    }
}
