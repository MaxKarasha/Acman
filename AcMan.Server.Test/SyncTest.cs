using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using AcMan.Server.Controllers;
using AcMan.Server.Core.DB;
using AcMan.Server.Models;
using System;
using AcMan.Server.Repositories;
using AcMan.Server.Integration.OData;
using System.Xml.Linq;
using System.Net;
using System.Xml;
using AcMan.Server.Integration.SyncStrategy;
using AcMan.Server.Integration.RemoteRepository;
using AcMan.Server.Integration.Converter;
using AcMan.Server.Core;

namespace AcMan.Server.Test
{
    [TestClass]
    public class SyncTest : BaseTest
    {
        [TestMethod]
        public void Sync_Test()
        {            
            using (var context = new AcManContext(OptionsRandom)) {//MsSql2016Options
                var userId = TestDemoData.CreateTestData(context);
                var bpm = DemoTS_ODBase;//MsSqlMKODBase
                var userRepository = new UserRepository(context);
                var activityRepository = new ActivityRepository(context);
                var synchronizationRepository = new SynchronizationRepository(context);
                var bpmOdataConverter = new BpmOdataConverter();
                var activityBpmOdataRepository = new ActivityBpmOdataRepository(bpm, bpmOdataConverter);

                var bpmonlineSyncStrategy = new BpmonlineSyncStrategy(
                    userRepository, 
                    activityRepository, 
                    synchronizationRepository, 
                    activityBpmOdataRepository
                );
                var startCount = activityRepository.GetAll().Count;
                var remoteActivity = new Activity {
                    UserId = userId,
                    Caption = "[Acman] Test remote activity " + AcmanHelper.GetCurrentDateTime().ToString()
                };
                remoteActivity.EndSystemRecordId = activityBpmOdataRepository.Add(remoteActivity);
                bpmonlineSyncStrategy.Sync();
                var endCount = activityRepository.GetAll().Count;
                Assert.IsTrue(endCount > startCount);
                activityBpmOdataRepository.Remove(remoteActivity);
            }            
        }
    }
}
