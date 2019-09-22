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
						return "https://upload.wikimedia.org/wikipedia/commons/2/2d/Mobile-Smartphone-icon.png";
					case "coffee":
						return "https://cdn4.iconfinder.com/data/icons/avatars-xmas-giveaway/128/coffee_zorro_avatar_cup-256.png";
					case "lunch":
						return "https://cdn0.iconfinder.com/data/icons/kameleon-free-pack-rounded/110/Food-Dome-256.png";
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
						return "https://i.pinimg.com/564x/a9/a9/ea/a9a9ea8016a7904e6a2f7533aa3d2af1.jpg";
					case "phone":
						return "https://i.pinimg.com/564x/07/b7/47/07b74764d9a4fd66cd361bb3f8132478.jpg";
					case "coffee":
						return "https://i.pinimg.com/564x/c0/f1/66/c0f1668c7f38c3dbdfbe3566ab5ce326.jpg";
					case "lunch":
						return "https://i.pinimg.com/564x/c1/49/f0/c149f00846e82cfdb99f856debd1899b.jpg";
					case "help":
						return "https://i.pinimg.com/564x/0b/69/55/0b695505cf4528565a01dcd2d287b8bd.jpg";
					default:
						return "";
				}
			}
		}
	}
}
