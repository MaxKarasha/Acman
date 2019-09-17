using AcMan.Server.Core.DB;
using AcMan.Server.Models;

namespace AcMan.Server.Repositories
{
    public class ActivityAdditionalInfoRepository : BaseRepository<ActivityAdditionalInfo>
    {
        public ActivityAdditionalInfoRepository(AcManContext context) : base(context) { }
    }
}
