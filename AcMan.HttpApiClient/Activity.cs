using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcMan.HttpApiClient
{
	public partial class Activity
	{
		[JsonIgnore]
		TimeSpan? _startTime = DateTime.Now.TimeOfDay;
		[JsonIgnore]
		public TimeSpan? StartTime {
			get {
				return _startTime;
			}
			set {
				if (_startTime != value)
				{
					_startTime = value;
					if (Start.HasValue && value.HasValue)
					{
						var date = Start.Value;
						Start = date.Date + value.Value;
					}
					RaisePropertyChanged();
				}
			}
		}
	}
}
