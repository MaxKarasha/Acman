using System;
using System.Collections.Generic;
using System.Text;

namespace AcMan.HttpApiClient
{
	public partial class Client
	{
		partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
		{
			request.Headers.Add("AcManKey", "test");
		}
	}
}
