using AcMan.Server.Models;
using AcMan.Server.Models.Base;
using System;
using System.Linq;

namespace AcMan.Server.Core.DB
{
    public static class DbInitializer
    {
        public static void Initialize(AcManContext context)
        {
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            if (context.Users.SingleOrDefault(s => s.Id == AcmanConstants.User.AcmanSUId) == null) {
                context.Users.Add(
                    new User{
                        Id = AcmanConstants.User.AcmanSUId,
                        Name = "Acman system user",
                        Login = "AcmanSU",
                        EntityState = AcmanEntityState.Added
                    }
                );
            }
            if (context.EndSystems.SingleOrDefault(s => s.Id == AcmanConstants.EndSystem.BpmonlineWorkTsi) == null) {
                context.EndSystems.Add(
                    new EndSystem{
                        Id = AcmanConstants.EndSystem.BpmonlineWorkTsi,
                        Name = "Bpm'online work TSI",
                        EntityState = AcmanEntityState.Added
                    }
                );
            }

            // Generate test data
            if (!context.Activities.Any())
            {
                context.Activities.Add(
                    new Activity
                    {
                        Id = Guid.NewGuid(),
                        Caption = "Рефакторинг кода по Лояльности",
                        UserId = AcmanConstants.User.AcmanSUId,
                        Status = ActivityStatus.InProgress,
                        Start = DateTime.Now,
                        EntityState = AcmanEntityState.Added
                    }
                );
                context.Activities.Add(
                    new Activity
                    {
                        Id = Guid.NewGuid(),
                        Caption = "Интеграция с SAP SM",
                        UserId = AcmanConstants.User.AcmanSUId,
                        Status = ActivityStatus.InPause,
                        EntityState = AcmanEntityState.Added
                    }
                );
                context.Activities.Add(
                    new Activity
                    {
                        Id = Guid.NewGuid(),
                        Caption = "Code review",
                        UserId = AcmanConstants.User.AcmanSUId,
                        Status = ActivityStatus.InPause,
                        Start = DateTime.Now,
                        EntityState = AcmanEntityState.Added
                    }
                );
                context.Activities.Add(
                    new Activity
                    {
                        Id = Guid.NewGuid(),
                        Caption = "Архитектура автопланирования, UML",
                        UserId = AcmanConstants.User.AcmanSUId,
                        Status = ActivityStatus.InPause,
                        EntityState = AcmanEntityState.Added
                    }
                );
            }

            context.SaveChanges();
        }
    }
}
