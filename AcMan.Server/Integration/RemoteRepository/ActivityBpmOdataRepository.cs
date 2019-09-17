using AcMan.Server.Attribute;
using AcMan.Server.Integration.Converter;
using AcMan.Server.Integration.OData;
using AcMan.Server.Models;
using AcMan.Server.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AcMan.Server.Integration.RemoteRepository
{
    public class ActivityBpmOdataRepository : BaseBpmOdataRepository<Activity>
    {
        public ActivityBpmOdataRepository(ODBase context, BpmOdataConverter bpmOdataConverter) : base(context, bpmOdataConverter) { }
    }
}
