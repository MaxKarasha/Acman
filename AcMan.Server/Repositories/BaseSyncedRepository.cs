using AcMan.Server.Core.DB;
using AcMan.Server.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Repositories
{
    public class BaseSyncedRepository<T> : BaseRepository<T>, ISyncedRepository<T> where T : class, IEntity, ISynchronizedEntity
    {
        public bool IsIntegration { get; set; } = false;

        public BaseSyncedRepository(AcManContext context) : base(context) { }

        public override Guid Add(T entity)
        {
            entity.IsIntegration = IsIntegration;
            if (entity.NeedUpdateRemoteIds) {
                UpdateRemoteColumns(entity);
                entity.NeedUpdateRemoteIds = false;
            }
            return base.Add(entity);
        }

        public override void Edit(T entity)
        {
            entity.IsIntegration = IsIntegration;
            if (entity.NeedUpdateRemoteIds) {
                UpdateRemoteColumns(entity);
                entity.NeedUpdateRemoteIds = false;
            }
            base.Edit(entity);
        }

        public virtual void UpdateRemoteColumns(T entity) { }
    }
}
