using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BMS_Test_Client.Extensions
{
    public static class Extensions
    {
        public static async Task<T> Get<T>(this HttpClient client, Uri path)
        {
            var result = await client.GetAsync(path);
            if (result.IsSuccessStatusCode)
            {
                var json = await result.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<T>(json);
                return values;
            }
            return default(T);
        }

        public static async Task<T> Put<T>(this HttpClient client, Uri path, object content = null)
        {
            string c = "";
            if (content != null) c = JsonConvert.SerializeObject(content);

            var result = await client.PutAsync(path, new StringContent(c, Encoding.UTF8, "application/json"));
            if (result.IsSuccessStatusCode)
            {
                var json = await result.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<T>(json);
                return values;
            }
            return default(T);
        }

        public static async Task<T> Post<T>(this HttpClient client, Uri path, object content = null)
        {
            string c = "";
            if (content != null) c = JsonConvert.SerializeObject(content);

            var result = await client.PostAsync(path, new StringContent(c, Encoding.UTF8, "application/json"));
            if (result.IsSuccessStatusCode)
            {
                var json = await result.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<T>(json);
                return values;
            }
            return default(T);
        }

        public static async Task<bool> PostNoContent(this HttpClient client, Uri path, object content = null)
        {
            string c = "";
            if (content != null) c = JsonConvert.SerializeObject(content);

            var result = await client.PostAsync(path, new StringContent(c, Encoding.UTF8, "application/json"));
            if (result.IsSuccessStatusCode)
                return true;
            return false;
        }

        public static async Task<bool> PutNoContent(this HttpClient client, Uri path, object content = null)
        {
            string c = "";
            if (content != null) c = JsonConvert.SerializeObject(content);

            var result = await client.PutAsync(path, new StringContent(c, Encoding.UTF8, "application/json"));
            if (result.IsSuccessStatusCode)
                return true;
            return false;
        }

        public static void Print(this Dictionary<string, object> dic)
        {
            Console.WriteLine("{");
            var keys = dic.Keys.ToList();
            keys.ForEach(key =>
            {
                Console.WriteLine("  " + key + " : " + dic[key]);
            });
            Console.Write("}");
        }

        public static void Print(this IEnumerable<Dictionary<string, object>> dics)
        {
            Console.WriteLine("[");
            dics.ToList().ForEach((dic) => { Print(dic); Console.Write(","); });
            Console.WriteLine("]");
        }
    }
}
