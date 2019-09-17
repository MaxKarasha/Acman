using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcMan.Server.Core;
using AcMan.Server.Core.DB;
using AcMan.Server.Core.KeyReader;
using AcMan.Server.Models;
using AcMan.Server.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AcMan.Server.Controllers
{
    public class GeneralOperation : BaseController<ActivityRepository, Activity>
    {
        private TagRepository _tagRepository;
        private ActivityAdditionalInfoRepository _activityAdditionalInfoRepository;

        public GeneralOperation(ActivityRepository repository, TagRepository tagRepository, ActivityAdditionalInfoRepository tagActivityAdditionalInfoRepository, AcManContext context) : base(repository, context) {
            _tagRepository = tagRepository;
            _activityAdditionalInfoRepository = tagActivityAdditionalInfoRepository;
        }

        [HttpPut]
        public void CreateTagFromActivity(Activity activity, string newTagName)
        {
            var activityAdditionalInfo = new ActivityAdditionalInfo(activity);
            _activityAdditionalInfoRepository.Add(activityAdditionalInfo);
            var tag = new Tag {
                Name = newTagName,
                ActivityAdditionalInfo = activityAdditionalInfo
            };
            _tagRepository.Add(tag);
        }
    }
}
