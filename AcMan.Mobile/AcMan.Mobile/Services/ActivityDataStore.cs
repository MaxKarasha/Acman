using AcMan.HttpApiClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AcMan.Mobile.Services
{
	public class ActivityDataStore : IDataStore<Activity>
	{
		private Client _client;

		public ActivityDataStore()
		{
			_client = new Client("http://10.0.2.2:54624");
		}
		public async Task<bool> AddItemAsync(Activity item)
		{
			return await _client.ApiActivitiesAddPostAsync(item) != Guid.Empty;
		}

		public Task<bool> DeleteItemAsync(string id)
		{
			throw new NotImplementedException();
		}

		public Task<Activity> GetItemAsync(string id)
		{
			return null;
		}
		public Task<Activity> GetLastItemAsync()
		{
			return null;
		}

		public async Task<IEnumerable<Activity>> GetItemsAsync(bool forceRefresh = false)
		{
			
			return await _client.ApiLastActivityGetGetAsync();
		}

		public Task<bool> UpdateItemAsync(Activity item)
		{
			throw new NotImplementedException();
		}
	}
}
