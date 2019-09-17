using AcMan.Server.Core;
using AcMan.Server.Integration.Converter;
using AcMan.Server.Integration.RemoteRepository;
using AcMan.Server.Models;
using AcMan.Server.Models.Base;
using AcMan.Server.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Integration.SyncStrategy
{
    public class BpmonlineSyncStrategy : ISyncStrategy//<T1, T2, T3> : ISyncStrategy where T1 : class, IEntity where T2 : BaseRepository<T1> where T3 : BaseRepository<T1>
    {
        private UserRepository _userRepository;
        private ActivityRepository _activityRepository;
        private SynchronizationRepository _synchronizationRepository;
        private ActivityBpmOdataRepository _activityBpmOdataRepository;
        private Guid _bpmonlineEndSystemId;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _info;
        public BpmonlineSyncStrategy(
            UserRepository userRepository, 
            ActivityRepository activityRepository, 
            SynchronizationRepository synchronizationRepository,
            ActivityBpmOdataRepository activityBpmOdataRepository)
        {
            _userRepository = userRepository;
            _activityRepository = activityRepository;
            _activityRepository.IsIntegration = true;
            _synchronizationRepository = synchronizationRepository;
            _activityBpmOdataRepository = activityBpmOdataRepository;
            _bpmonlineEndSystemId = AcmanConstants.EndSystem.BpmonlineWorkTsi;
            _startDate = _synchronizationRepository.GetLastSyncDate();
            _endDate = AcmanHelper.GetCurrentDateTime();
            _info = string.Empty;
        }

        public string Info => _info;

        public void Sync()
        {
            try {
                ICollection<User> bpmWorkUsers = _userRepository.GetUsersByEndSystem(_bpmonlineEndSystemId);
                _info += "[Users for sync: " + bpmWorkUsers.Count.ToString() + "] ";
                foreach (User user in bpmWorkUsers) {
                    SyncUser(user);
                }
            }
            catch (Exception e) {
                _info = "Error: " + e.Message + " " + _info;
            }
            finally {
                _synchronizationRepository.Add(
                   new Synchronization {
                       EndSystemId = _bpmonlineEndSystemId,
                       StartPeriod = _startDate,
                       EndPeriod = _endDate,
                       Info = _info,
                       Duration = (int)(AcmanHelper.GetCurrentDateTime() - _endDate).TotalSeconds
                   }
               );
            }
            
        }

        public void SyncUser(User user)
        {
            try {
                _info += "[User: " + user.Name + "; ";
                SyncUserActivityes(user, user.LastSyncDate ?? AcmanHelper.GetCurrentDateTime().AddDays(-7));
                user.LastSyncDate = _endDate;
                _userRepository.Edit(user);
            } catch (Exception e) {
                _info = "Error during sync user: " + e.Message + " " + _info;
            } finally {
                _info += "]; ";
            }
        }

        public void SyncUserActivityes(User user, DateTime startDate)
        {
            ICollection<Activity> remoteActivityes = 
                _activityBpmOdataRepository.GetAllChangedBetweenDatesForUser(startDate, _endDate, user);
            _info += "RemoteActivityes: " + remoteActivityes.Count.ToString() + "; ";
            ICollection<Activity> acmanChangedActivityes =
                _activityRepository.GetUnsyncedForUser(user);            
            foreach (var remoteActivity in remoteActivityes) {
                var acmanActivity = 
                    acmanChangedActivityes
                    .Where(a => a.EndSystemRecordId == remoteActivity.EndSystemRecordId)
                    .FirstOrDefault()
                    ??
                    _activityRepository.GetByRemoteRecordId(remoteActivity.EndSystemRecordId);
                if (acmanActivity != null) {
                    if (acmanActivity.ModifiedOn < remoteActivity.ModifiedOn) {
                        acmanChangedActivityes.Remove(acmanActivity);
                        AcmanHelper.MergeSyncedObjects(acmanActivity, remoteActivity);
                        _activityRepository.Edit(acmanActivity);
                    }                    
                } else {
                    _activityRepository.Add(remoteActivity);
                }
            }
            _info += "AcmanActivityes: " + acmanChangedActivityes.Count.ToString() + "; ";
            foreach (var acmanActivity in acmanChangedActivityes) {
                if (acmanActivity.EndSystemRecordId == null) {
                    Guid endSystemRecordId = _activityBpmOdataRepository.Add(acmanActivity);
                    acmanActivity.EndSystemRecordId = endSystemRecordId;
                } else {
                    _activityBpmOdataRepository.Edit(acmanActivity);
                }
                _activityRepository.Edit(acmanActivity);
            }
        }
    }
}
