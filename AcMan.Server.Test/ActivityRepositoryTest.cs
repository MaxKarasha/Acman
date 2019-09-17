using AcMan.Server.Core.DB;
using AcMan.Server.Models;
using AcMan.Server.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AcMan.Server.Test
{
    [TestClass]
    public class ActivityRepositoryTest : BaseTest
    {
        [TestMethod]
        public void Add_Get_Activity_From_Repository()
        {
            var userId = Guid.NewGuid();
            using (var context = new AcManContext(Options))
            {
                var userRepository = new UserRepository(context);
                var activityRepository = new ActivityRepository(context);

                var user = new User {
                    Id = userId,
                    Name = "Test " + userId.ToString()
                };
                var activity = new Activity
                {
                    Caption = "Test activity",
                    User = user
                };

                userRepository.Add(user);
				var activityId = activityRepository.Add(
                    activity
                );

                var resultActivity = activityRepository.Get(activityId);
                Assert.IsNotNull(resultActivity);
            }
        }

        [TestMethod]
        public void Add_Get_Activity_Tag_TagInActivity_From_Repository()
        {
            using (var context = new AcManContext(OptionsRandom)) {
                string randomTagName = "Tag " + Guid.NewGuid();
                var activity = new Activity {
                    Caption = "Test activity"
                };
                var tag = new Tag {
                    Name = randomTagName
                };

                var activityRepository = new ActivityRepository(context);
                var tagRepository = new TagRepository(context);
                var tagId = tagRepository.Add(tag);
                var activityId = activityRepository.Add(activity);

                var tagInActivity = new TagInActivity {
                    TagId = tagId,
                    ActivityId = activityId
                };
                var tagInActivityRepository = new TagInActivityRepository(context);
                tagInActivityRepository.Add(tagInActivity);

                var resultActivity = activityRepository.GetWithTags(activityId);
                Assert.IsNotNull(resultActivity.Tags);
                Assert.IsTrue(((Tag[])resultActivity.Tags)[0].Name.Equals(randomTagName));                
            }
        }

        [TestMethod]
        public void Add_GetWithRelation_Activity_With_Account_From_Repository()
        {
            using (var context = new AcManContext(OptionsRandom)) {
                string randomAccountName = "Account " + Guid.NewGuid();                
                var account = new Account {
                    Name = randomAccountName
                };                
                var accountRepository = new AccountRepository(context);
                var accountId = accountRepository.Add(account);

                var activity = new Activity {
                    Caption = "Test activity",
                    AccountId = accountId
                };
                var activityRepository = new ActivityRepository(context);
                var activityId = activityRepository.Add(activity);

                var resultActivity = activityRepository.GetWithRelation(activityId);
                Assert.IsTrue(resultActivity.AccountId == accountId);
                Assert.IsTrue(resultActivity.Account.Id == accountId);
            }
        }

        [TestMethod]
        public void Add_Get_Activity_Tags_From_Repository()
        {
            using (var context = new AcManContext(OptionsRandom)) {
                string randomTagName = "Tag " + Guid.NewGuid();
                var tag = new Tag {
                    Name = randomTagName
                };
                var activityTags = new Collection<Tag> { tag };
                var activity = new Activity {
                    Caption = "Test activity",
                    Tags = activityTags
                };                

                var activityRepository = new ActivityRepository(context);
                var tagRepository = new TagRepository(context);
                var tagId = tagRepository.Add(tag);
                var activityId = activityRepository.Add(activity);

                var resultActivity = activityRepository.GetWithTags(activityId);
                Assert.IsNotNull(resultActivity.Tags);
                Assert.IsTrue(((Tag[])resultActivity.Tags)[0].Name.Equals(randomTagName));
            }
        }

        //[TestMethod]
        //public void Get_Activity_FilteredByTags_Repository()
        //{
        //    using (var context = new AcManContext(OptionsRandom)) {
        //        string randomTagName = "Tag " + Guid.NewGuid();
        //        var tag = new Tag {
        //            Name = randomTagName
        //        };
        //        var activityTags = new Collection<Tag> { tag };
        //        var activity = new Activity {
        //            Caption = "Test activity",
        //            Tags = activityTags
        //        };

        //        var activityRepository = new ActivityRepository(context);
        //        var tagRepository = new TagRepository(context);
        //        var tagId = tagRepository.Add(tag);
        //        var activityId = activityRepository.Add(activity);

        //        var resultActivity = activityRepository.FilterByTag(tag);
        //        Assert.IsNotNull(resultActivity);
        //        Assert.IsTrue(resultActivity.Count > 0);
        //    }
        //}
    }
}
