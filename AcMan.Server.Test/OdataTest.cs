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

namespace AcMan.Server.Test
{
    [TestClass]
    public class OdataTest : BaseTest
    {
        [TestMethod]
        public void Sync_Add_Edit_Activity_To_Bpm()
        {
            string url = "http://m-karasha/Bandle_7120_Demo_D";
            string login = "Supervisor";
            string password = "Supervisor";
            var bpm = new ODBase(url, login, password);
            var count = bpm.GetCollectionSize("Activity");

            //var a = bpm.GetPage(bpm.GetCollectionPageUrl("Activity") + "Collection(guid'9cfb2535-19bf-4e63-b2dd-1ba78decc558')");

            Guid activityId = Guid.NewGuid();
            ODObject record = ODObject.NewObject("Activity");
            //try {
            //    record = new ODObject(bpm, "Activity", activityId.ToString());
            //} catch (Exception e) {
            //    record = ODObject.NewObject("Activity");
            //}
            record["Title"] = "Test MK 231";
            record["OwnerId"] = "410006e1-ca4e-4502-a9ec-e54d922d2c00";
            record["StatusId"] = "384d4b84-58e6-df11-971b-001d60e938c6";
            record["StartDate"] = "2015-12-15T16:57:12.3599795Z";
            record["DueDate"] = "2015-12-15T18:57:12.3599795Z";

            var result = record.Update(bpm);
            Guid id = ODObject.GetGuidFromEntityUrl(result, "Activity");

            //ODObject record2 = new ODObject(bpm, "Activity", id.ToString());
            ODObject record2 = ODObject.NewObject("Activity");
            record2.Guid = id.ToString();
            record2["Title"] = "Test MK 231 - NEW";
            record2.Update(bpm);

            var newCount = bpm.GetCollectionSize("Activity");
            Assert.IsTrue(newCount > count);
        }
    }
}
