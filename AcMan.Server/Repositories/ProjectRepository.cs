using AcMan.Server.Core.DB;
using AcMan.Server.Models;

namespace AcMan.Server.Repositories
{
    public class ProjectRepository : BaseLookupEntityRepository<Project>
    {
        public ProjectRepository(AcManContext context) : base(context) { }
    }
}
