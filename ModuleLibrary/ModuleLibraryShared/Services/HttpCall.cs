using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace ModuleLibraryShared.Services
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
        /*
        public class CallManager<ServerObject, ReturnType> where ServerObject : class where ReturnType : class{
			public async Task<ReturnType> Call(CallType callType, string path, Func<ServerObject, ReturnType> successA = null, Func<ReturnType> errorA = null, object content = null, JsonConverter jsonConverter = null, UIViewController vc = null)
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
        } */

		public class CallManager<ServerObject> where ServerObject : class 
		{
            public async Task<ServerObject> Call(CallType callType, string path, object content = null, Action<ServerObject> successA = null, Action errorA = null, JsonConverter jsonConverter = null)
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
						if (jsonConverter != null) returnObj = JsonConvert.DeserializeObject<ServerObject>(result, jsonConverter);
						else returnObj = JsonConvert.DeserializeObject<ServerObject>(result);

                        if (successA != null) 
                            successA.Invoke(returnObj);
						
						return returnObj as ServerObject;
                    } else {
						var result = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine("Response error: " + response.StatusCode + ", " + result);
                    }
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine("CallManager error: " + ex.Message);
				}
				if (errorA != null)
					errorA.Invoke();
				
				return default(ServerObject);
			}
		}

        public class CallManager {
            public async Task<bool> Call(CallType callType, string path, object content = null, Action successA = null, Action errorA = null)
            {
                HttpClient httpClient = GetHttpClient();
                string c = "";
                if (content != null) {
					c = JsonConvert.SerializeObject(content);
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
                        if (successA != null)
                            successA.Invoke();
                        
                        return true;
                    } else {
						var result = await response.Content.ReadAsStringAsync();
						System.Diagnostics.Debug.WriteLine("Response error: " + response.StatusCode + ", " + result);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("CallManager error: " + ex.Message);
                    System.Diagnostics.Debug.WriteLine("CallManager path: " + path);
                    System.Diagnostics.Debug.WriteLine("CallManager content: " + c);
                }
				if (errorA != null)
					errorA.Invoke();
				
                return false;
            }
        }
		public enum CallType { Post, Get, Put, Delete }
	}
}