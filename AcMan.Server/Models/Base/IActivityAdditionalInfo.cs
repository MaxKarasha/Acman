using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Models.Base
{
    interface IActivityAdditionalInfo
    {
        string JiraUrl { get; set; }
        Guid? AccountId { get; set; }
        Account Account { get; set; }
        Guid? ProjectId { get; set; }
        Project Project { get; set; }
        Guid? WorkInProjectId { get; set; }
        WorkInProject WorkInProject { get; set; }
    }
}
