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
    public class UserRepositoryTest : BaseTest
    {
        [TestMethod]
        public void Add_Get_User_From_Repository()
        {
            var userId = Guid.NewGuid();
            using (var context = new AcManContext(Options))
            {
                var userRepository = new UserRepository(context);
                var returnedId = userRepository.Add(new User { Id = userId, Name = "Test " + userId.ToString() });
                Assert.AreEqual(userId, returnedId);

                var user = userRepository.Get(userId);
                Assert.IsNotNull(user);
            }
        }

        [TestMethod]
        public void Add_2_Users_GetAll_From_Repository()
        {
            using (var context = new AcManContext(OptionsRandom))
            {
                var userRepository = new UserRepository(context);
                userRepository.Add(new User { Name = "Test 1"});
                userRepository.Add(new User { Name = "Test 2" }); 

                var users = userRepository.GetAll();
                Assert.IsTrue(users.Count == 2);
            }
        }

        [TestMethod]
        public void Add_Get_Edit_User_From_Repository()
        {
            var userId = Guid.NewGuid();
            var name = "Test " + userId.ToString();
            using (var context = new AcManContext(Options))
            {
                var userRepository = new UserRepository(context);
                userRepository.Add(new User { Id = userId, Name = name });

                var user = userRepository.Get(userId);
                var name1 = user.Name;

                user.Name = name + "2";
                userRepository.Edit(user);

                var user2 = userRepository.Get(userId);
                var name2 = user2.Name;
                Assert.AreNotEqual(name1, name2);
                Assert.AreEqual(name2, name + "2");
            }
        }

        [TestMethod]
        public void Add_Get_Remove_Entity_User_From_Repository()
        {
            var userId = Guid.NewGuid();
            var name = "Test " + userId.ToString();
            using (var context = new AcManContext(Options))
            {
                var userRepository = new UserRepository(context);
                userRepository.Add(new User { Id = userId, Name = name });

                var user = userRepository.Get(userId);
                userRepository.Remove(user);

                var user2 = userRepository.Get(userId);
                Assert.IsNull(user2);
            }
        }

        [TestMethod]
        public void Add_Get_Remove_By_Key_User_From_Repository()
        {
            var userId = Guid.NewGuid();
            var name = "Test " + userId.ToString();
            using (var context = new AcManContext(Options))
            {
                var userRepository = new UserRepository(context);
                userRepository.Add(new User { Id = userId, Name = name });
                userRepository.Remove(userId);
                var user2 = userRepository.Get(userId);
                Assert.IsNull(user2);
            }
        }
    }
}
