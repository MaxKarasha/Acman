using AcMan.Server.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcMan.Server.Models
{
    public class ActivityAdditionalInfo : BaseEntityActivityAdditionalInfo
    {
        public ActivityAdditionalInfo(Activity activity)
        {
            JiraUrl = activity.JiraUrl;
            Account = activity.Account;
            Project = activity.Project;
            WorkInProject = activity.WorkInProject;
        }
        public ICollection<Tag> Tags { get; set; }
    }
}
