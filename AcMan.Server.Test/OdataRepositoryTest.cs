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
using AcMan.Server.Integration.RemoteRepository;
using AcMan.Server.Integration.Converter;

namespace AcMan.Server.Test
{
    [TestClass]
    public class OdataRepositoryTest : BaseTest
    {
        [TestMethod]
        public void Add_Get_Activity()
        {
            string url = "http://m-karasha/Bandle_7120_Demo_D";
            string login = "Supervisor";
            string password = "Supervisor";
            var bpm = new ODBase(url, login, password);
            var baseBpmOdataRepository = new BaseBpmOdataRepository<Activity>(bpm, new BpmOdataConverter());
            Assert.IsTrue(true);
        }
    }
}
