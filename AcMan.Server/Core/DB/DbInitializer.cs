using AcMan.Server.Models;
using AcMan.Server.Models.Base;
using AcMan.Server.Repositories;
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
                var demoUser = new User
                {
                    Id = AcmanConstants.User.AcmanSUId,
                    Name = "Acman system user",
                    Login = "AcmanSU",
                    EndSystemRecordId = new Guid("410006e1-ca4e-4502-a9ec-e54d922d2c00"),
                    EntityState = AcmanEntityState.Added
                };
                context.Users.Add(demoUser);
                var userInSystem = new UserInSystem
                {
                    User = demoUser,
                    Key = AcmanConstants.DemoKeyName
                };
                var userInSystemRepository = new UserInSystemRepository(context);
                userInSystemRepository.Add(userInSystem);
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
                        Status = ActivityStatus.New,
                        EntityState = AcmanEntityState.Added
                    }
                );
                context.Activities.Add(
                    new Activity
                    {
                        Id = Guid.NewGuid(),
                        Caption = "Code review",
                        UserId = AcmanConstants.User.AcmanSUId,
                        Status = ActivityStatus.New,
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
