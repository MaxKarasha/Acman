using AcMan.Server.Attribute;
using AcMan.Server.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcMan.Server.Models.Base
{
    [NotMapped]
    public abstract class BaseEntityActivityAdditionalInfo : BaseEntity, IActivityAdditionalInfo
    {
        //[RemoteEntityColumnName("JiraUrl")]
        public string JiraUrl { get; set; }
        public Guid? AccountId { get; set; }
        [NotMapped]
        [RemoteEntityColumnName("AccountId")]
        public virtual Account Account { get; set; }
        public Guid? ProjectId { get; set; }
        [NotMapped]        
        public virtual Project Project { get; set; }
        public Guid? WorkInProjectId { get; set; }
        [NotMapped]
        //[RemoteEntityColumnName("WorkInProjectId")]
        public virtual WorkInProject WorkInProject { get; set; }        
    }
}
