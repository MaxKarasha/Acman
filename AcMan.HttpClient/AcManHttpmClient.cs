using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AcMan.HttpApiClient
{
	public class AcManHttpClient
	{
		HttpClient _client = new HttpClient();
		public T Get<T>(string url)
		{
			var result = _client.GetStringAsync(url);
			result.Wait();
			return JsonConvert.DeserializeObject<T>(result.Result);
		}
	}
}
