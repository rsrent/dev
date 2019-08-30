using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ModuleLibraryShared.Services
{
	public static class Extensions
	{
		public static async Task<T> Get<T>(this HttpClient client, string path, Action<T> successA = null, Action errorA = null, JsonConverter converter = null) where T : class
		{
            return await Call<T>(client, CallType.Get, path, successA: successA, errorA: errorA, jsonConverter: converter);
		}

		public static async Task Put(this HttpClient client, string path, object content = null, Action successA = null, Action errorA = null)
		{
			await Call(client, CallType.Put, path, content, successA: successA, errorA: errorA);
		}

        public static async Task<T> Post<T>(this HttpClient client, string path, object content = null, Action<T> successA = null, Action errorA = null) where T : class
		{
			return await Call<T>(client, CallType.Post, path, content, successA: successA, errorA: errorA);
		}

		public static async Task Post(this HttpClient client, string path, object content = null, Action successA = null, Action errorA = null)
		{
			await Call(client, CallType.Post, path, content, successA: successA, errorA: errorA);
		}

		public static async Task Delete(this HttpClient client, string path, Action successA = null, Action errorA = null)
		{
			await Call(client, CallType.Delete, path, successA: successA, errorA: errorA);
		}

		private static async Task<T> Call<T>(HttpClient client, CallType callType, string path, object content = null, Action<T> successA = null, Action errorA = null, JsonConverter jsonConverter = null) where T : class
		{
			string c = "";
			if (content != null) c = JsonConvert.SerializeObject(content);

            HttpResponseMessage response = null;

			try
			{
				if (callType == CallType.Post) response = await client.PostAsync(new Uri(path), new StringContent(c, Encoding.UTF8, "application/json"));
				else if (callType == CallType.Get) response = await client.GetAsync(new Uri(path));
				else if (callType == CallType.Put) response = await client.PutAsync(new Uri(path), new StringContent(c, Encoding.UTF8, "application/json"));
				else if (callType == CallType.Delete) response = await client.DeleteAsync(new Uri(path));

				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					T returnObj = null;
					if (jsonConverter != null) returnObj = JsonConvert.DeserializeObject<T>(result, jsonConverter);
					else returnObj = JsonConvert.DeserializeObject<T>(result);

					if (successA != null)
						successA.Invoke(returnObj);

                    return returnObj;
				}
				else
				{
					var result = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("Response error: " + response.StatusCode + ", " + result + ", " + path);
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("CallManager error: " + ex.Message);
			}
			if (errorA != null)
				errorA.Invoke();
            return default(T);
		}

		private static async Task Call(HttpClient client, CallType callType, string path, object content = null, Action successA = null, Action errorA = null)
		{
			string c = "";
			if (content != null) c = JsonConvert.SerializeObject(content);
			try
			{
				HttpResponseMessage response = null;
				if (callType == CallType.Post) response = await client.PostAsync(new Uri(path), new StringContent(c, Encoding.UTF8, "application/json"));
				else if (callType == CallType.Get) response = await client.GetAsync(new Uri(path));
				else if (callType == CallType.Put) response = await client.PutAsync(new Uri(path), new StringContent(c, Encoding.UTF8, "application/json"));
				else if (callType == CallType.Delete) response = await client.DeleteAsync(new Uri(path));

				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();

					if (successA != null)
						successA.Invoke();

					return;
				}
				else
				{
					var result = await response.Content.ReadAsStringAsync();
					System.Diagnostics.Debug.WriteLine("Response error: " + response.StatusCode + ", " + result + ", " + path);
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("CallManager error: " + ex.Message);
			}
			if (errorA != null)
				errorA.Invoke();
		}

		enum CallType
		{
			Post, Get, Put, Delete
		}
	}
}
