using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using AcMan.Server.Controllers;
using AcMan.Server.Core.DB;
using AcMan.Server.Models;
using System;
using AcMan.Server.Repositories;

namespace AcMan.Server.Test
{
    [TestClass]
    public class TagControllerTest : BaseTest
    {
        [TestMethod]
        public void Add_Get_Remove_Tag()
        {
            using (var context = new AcManContext(Options)) {
                string randomTagName = "Tag " + Guid.NewGuid();
                var tag = new Tag {
                    Name = randomTagName
                };

                var tagRepository = new TagRepository(context);
                var controller = new TagController(tagRepository, context);

                var tagId = controller.Add(tag);
                var newTag = controller.Get(tagId);

                Assert.AreEqual(newTag.Name, randomTagName);

                controller.Remove(tagId);
                var removedTag = controller.Get(tagId);

                Assert.IsNull(removedTag);
            }
        }
    }
}
