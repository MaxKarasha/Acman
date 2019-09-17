using AcMan.Server.Core.DB;
using AcMan.Server.Models;

namespace AcMan.Server.Repositories
{
    public class TagRepository : BaseLookupEntityRepository<Tag>
    {
        public TagRepository(AcManContext context) : base(context) { }
    }
}
