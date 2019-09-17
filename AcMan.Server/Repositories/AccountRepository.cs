using AcMan.Server.Core.DB;
using AcMan.Server.Models;

namespace AcMan.Server.Repositories
{
    public class AccountRepository : BaseLookupEntityRepository<Account>
    {
        public AccountRepository(AcManContext context) : base(context) { }
    }
}
