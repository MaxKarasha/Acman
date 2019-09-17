using AcMan.Server.Core;
using AcMan.Server.Core.DB;
using AcMan.Server.Models;
using AcMan.Server.Models.Base;
using AcMan.Server.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcMan.Server.Test
{
    class TestDemoData
    {
        public static Guid CreateSystemUser(AcManContext context)
        {
            var userRepository = new UserRepository(context);
            var user = new User {
                Id = AcmanConstants.User.AcmanSUId,
                Name = "Acman system user",
                Login = "AcmanSU",
                EndSystemRecordId = new Guid("410006e1-ca4e-4502-a9ec-e54d922d2c00"), //Supervisor
                EntityState = AcmanEntityState.Added
            };
            return userRepository.Add(user);
        }

        public static Guid CreateBpmSystem(AcManContext context)
        {
            var endSystemRepository = new EndSystemRepository(context);
            var endSystem = new EndSystem {
                Id = AcmanConstants.EndSystem.BpmonlineWorkTsi,
                Name = "Bpm'online work TSI",
                EntityState = AcmanEntityState.Added
            };
            return endSystemRepository.Add(endSystem);
        }

        public static Guid CreateActivityToUser(AcManContext context, Guid userId)
        {
            var activityRepository = new ActivityRepository(context);
            var activity = new Activity {
                UserId = userId,
                Caption = "[Acman] Test activity " + AcmanHelper.GetCurrentDateTime().ToString(),
                EndSystemRecordId = new Guid("8cb830fb-50d8-44ad-819f-133b713fce42"),
                EntityState = AcmanEntityState.Added
            };
            return activityRepository.Add(activity);
        }

        public static Guid CreateTestData(AcManContext context)
        {
            var userId = CreateSystemUser(context);
            CreateActivityToUser(context, userId);
            var systemId = CreateBpmSystem(context);
            var userInSystemRepository = new UserInSystemRepository(context);
            var userInSystem = new UserInSystem {
                UserId = userId,
                EndSystemId = systemId,
                Key = "TestKey",
                EntityState = AcmanEntityState.Added
            };
            userInSystemRepository.Add(userInSystem);
            return userId;
        }
    }
}
