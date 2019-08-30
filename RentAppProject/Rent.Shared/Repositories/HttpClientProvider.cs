using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RentApp
{
    public class HttpClientProvider
    {
        public HttpClient Client { get; set; }
        Settings Settings;

        public HttpClientProvider(Settings settings)
        {
            Settings = settings;
            RestartClient();
        }

        public void RestartClient() {
            Client = new HttpClient()
            {
                BaseAddress = Settings.ApiBaseAddress
            };
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
