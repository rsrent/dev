using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Rent.Helpers
{
	public class HttpCall
	{
        static HttpClient HttpClient = null;
        public static HttpClient GetHttpClient() {
            if(HttpClient == null) {
                HttpClient = new HttpClient();

				JsonConvert.DefaultSettings = () => new JsonSerializerSettings
				{
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore
				};
            }
            return HttpClient;
        }

        public class CallManager<ServerObject, ReturnType> where ServerObject : class where ReturnType : class{
			public async Task<ReturnType> Call(CallType callType, string path, Func<ServerObject, ReturnType> successA = null, Func<ReturnType> errorA = null, object content = null, JsonConverter jsonConverter = null)
			{
                HttpClient httpClient = GetHttpClient();
				string c = "";
                if (content != null) c = JsonConvert.SerializeObject(content);

				try
				{
					HttpResponseMessage response = null;
					if (callType == CallType.Post) response = await httpClient.PostAsync(new Uri(path), new StringContent(c, Encoding.UTF8, "application/json"));
					else if (callType == CallType.Get) response = await httpClient.GetAsync(new Uri(path));
					else if (callType == CallType.Put) response = await httpClient.PutAsync(new Uri(path), new StringContent(c, Encoding.UTF8, "application/json"));
					else if (callType == CallType.Delete) response = await httpClient.DeleteAsync(new Uri(path));

					if (response.IsSuccessStatusCode)
					{
						var result = await response.Content.ReadAsStringAsync();
                        ServerObject returnObj = null;
                        if(jsonConverter != null) returnObj = JsonConvert.DeserializeObject<ServerObject>(result, jsonConverter);
						else returnObj = JsonConvert.DeserializeObject<ServerObject>(result);
                        if(successA != null)return successA.Invoke(returnObj);
                        return returnObj as ReturnType;
					}
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.Message);
				}
                if (errorA != null) return errorA.Invoke();
				return default(ReturnType);
			}
        }

		public class CallManager<ServerObject> where ServerObject : class
		{
			public async Task<ServerObject> Call(CallType callType, string path, Func<ServerObject, ServerObject> successA = null, Func<ServerObject> errorA = null, object content = null)
			{
				HttpClient httpClient = GetHttpClient();
				string c = "";
				if (content != null) c = JsonConvert.SerializeObject(content);

				try
				{
					HttpResponseMessage response = null;
					if (callType == CallType.Post) response = await httpClient.PostAsync(new Uri(path), new StringContent(c, Encoding.UTF8, "application/json"));
					else if (callType == CallType.Get) response = await httpClient.GetAsync(new Uri(path));
					else if (callType == CallType.Put) response = await httpClient.PutAsync(new Uri(path), new StringContent(c, Encoding.UTF8, "application/json"));
					else if (callType == CallType.Delete) response = await httpClient.DeleteAsync(new Uri(path));

					if (response.IsSuccessStatusCode)
					{
						var result = await response.Content.ReadAsStringAsync();
						var returnObj = JsonConvert.DeserializeObject<ServerObject>(result);
						if (successA != null) return successA.Invoke(returnObj);
						return returnObj as ServerObject;
					}
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.Message);
				}
				if (errorA != null) return errorA.Invoke();
				return default(ServerObject);
			}
		}



        public class CallManager {
            public async Task<bool> Call(CallType callType, string path, object content = null)
            {
                HttpClient httpClient = GetHttpClient();
                string c = "";
                if (content != null) {
					c = JsonConvert.SerializeObject(content);
                    System.Diagnostics.Debug.WriteLine(c);
                }

                try
                {
                    HttpResponseMessage response = null;
                    if (callType == CallType.Post) response = await httpClient.PostAsync(new Uri(path), new StringContent(c, Encoding.UTF8, "application/json"));
                    else if (callType == CallType.Get) response = await httpClient.GetAsync(new Uri(path));
                    else if (callType == CallType.Put) response = await httpClient.PutAsync(new Uri(path), new StringContent(c, Encoding.UTF8, "application/json"));
                    else if (callType == CallType.Delete) response = await httpClient.DeleteAsync(new Uri(path));

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }

                    try {
						var result = await response.Content.ReadAsStringAsync();
						System.Diagnostics.Debug.WriteLine(result);
                    } catch (Exception exc) {
                        
                    }

                    System.Diagnostics.Debug.WriteLine("response.IsSuccessStatusCode: " + response.IsSuccessStatusCode);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("CallManager error: " + ex.Message);
                }
                return false;
            }
        }
		public enum CallType { Post, Get, Put, Delete }
	}
}
