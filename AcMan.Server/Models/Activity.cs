using AcMan.Server.Attribute;
using AcMan.Server.Core;
using AcMan.Server.Models.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Models
{
    public enum ActivityStatus
    {
        New,
        InPause,
        InProgress,        
        Done
    }

    [Table("Activity")]
    [RemoteEntityName("Activity")]
    public class Activity : BaseSynchronizedEntityActivityAdditionalInfo
    {
        [RemoteEntityColumnName("Title")]
        public string Caption { get; set; }
        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        [RemoteEntityColumnName("OwnerId")]
        public virtual User User { get; set; }
        [RemoteEntityColumnName("StartDate")]
        public DateTime? Start { get; set; }
        [RemoteEntityColumnName("DueDate")]
        public DateTime? End { get; set; }
        [RemoteEntityColumnName("StatusId", "StatusMap")]
        public ActivityStatus Status { get; set; }
        [NotMapped]
        public ICollection<Tag> Tags {
            get => TagInActivities?.Where(p => p.ActivityId == this.Id).Select(p => p.Tag).ToArray();
            set {
                TagInActivities = TagInActivities ?? new Collection<TagInActivity>();
                foreach (var tag in value) {
                    TagInActivities.Add(
                        new TagInActivity {
                            TagId = tag.Id,
                            Tag = tag,
                            ActivityId = Id,
                            Activity = this
                        }
                    );
                }
            }
        }
        public virtual ICollection<TagInActivity> TagInActivities { get; set; }

		public string Source { get; set; }

		public string SourceIcon {
			get {
				switch (Source?.ToLower()) {
					case "telegram":
						return "https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/Telegram_2019_Logo.svg/1200px-Telegram_2019_Logo.svg.png";
					case "jira":
						return "https://www.shareicon.net/download/2017/06/23/887698_management.ico";
					case "email":
						return "https://www.freepnglogos.com/uploads/email-png/mail-message-email-send-image-pixabay-5.png";
					case "phone":
						return "https://p7.hiclipart.com/preview/661/236/915/computer-icons-smartphone-mobile-app-clip-art-file-mobile-smartphone-icon-wikimedia-commons.jpg";
					case "coffee":
						return "https://freeiconshop.com/wp-content/uploads/edd/coffee-takeaway-flat.png";
					case "lunch":
						return "http://www.familiekerk.com/wp-content/uploads/2014/09/AWT-Meal.png";
					case "help":
						return "http://www.sclance.com/pngs/help-icon-png/help_icon_png_656596.png";
					default:
						return "http://www.sclance.com/pngs/strong-png/strong_png_1323879.png";
				}
			}
		}
		public string BackgroundIcon {
			get {
				switch (Source?.ToLower()) {
					case "telegram":
						return "";
					case "jira":
						return "https://i.pinimg.com/564x/cf/56/ae/cf56aea62b2bc4aceca0086e7d26acea.jpg";
					case "email":
						return "";
					case "phone":
						return "";
					case "coffee":
						return "https://i.pinimg.com/564x/27/0c/79/270c79fde612df07656595f56fbade9f.jpg";
					case "lunch":
						return "https://i.pinimg.com/564x/29/4e/ba/294eba95c7f1174e8f6e8707fca5ff5f.jpg";
					case "help":
						return "https://i.pinimg.com/564x/0b/69/55/0b695505cf4528565a01dcd2d287b8bd.jpg";
					default:
						return "";
				}
			}
		}
	}
}
