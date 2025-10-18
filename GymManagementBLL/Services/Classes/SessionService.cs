using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Quic;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    internal class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

       
        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory();
            if (!Sessions.Any()) return [];

            var MappedSessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(Sessions);
            foreach (var S in MappedSessions)
               S.AvailableSlots = S.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(S.Id);
            return MappedSessions;

        }

        public SessionViewModel GetSessionDetails(int sessionId)
        {
            var Session = _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionId);
            if(Session is null) return null;
            
            var MappedSession = _mapper.Map<Session, SessionViewModel>(Session);
            MappedSession.AvailableSlots 
                = MappedSession.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(MappedSession.Id);
            return MappedSession;

        }

        public bool CreateSession(CreateSessionViewModel CreatedSession)
        {
            try
            {
                if(!TrainerExists(CreatedSession.TrainerId) || !CategoryExists(CreatedSession.CategoryId)) return false;
                if(!IsDateValid(CreatedSession.StartDate, CreatedSession.EndDate)) return false;
                if(CreatedSession.Capacity < 1 || CreatedSession.Capacity > 25) return false;

                var SessionEntity = _mapper.Map<Session>(CreatedSession);
                _unitOfWork.SessionRepository.Add(SessionEntity);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Create Session Failed : {ex}");
                return false;
            }
        }

        public UpdateSessionViewModel? GetSessionToUpdate(int SessionId)
        {
            var Session = _unitOfWork.SessionRepository.GetById(SessionId);
            if (!IsSessionAvailableForUpdate(Session)) return null;

             return _mapper.Map<UpdateSessionViewModel>(Session);
        }

        public bool UpdateSession(int SessionId , UpdateSessionViewModel UpdatedSession)
        {
            try
            {
                var session = _unitOfWork.SessionRepository.GetById(SessionId);
                if (!IsSessionAvailableForUpdate(session!)) return false;
                if (!TrainerExists(UpdatedSession.TrainerId)) return false;
                if(!IsDateValid(UpdatedSession.StartDate, UpdatedSession.EndDate)) return false;
                
                
                _mapper.Map(UpdatedSession , session);
                session.UpdatedAt = DateTime.Now;
                _unitOfWork.SessionRepository.Update(session);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Update Session Falied {ex}");
                return false;
            }
        }

        public bool RemoveSession(int SessionId)
        {
            try
            {
                var session = _unitOfWork.SessionRepository.GetById(SessionId);
                if (!IsSessionAvailableForDelete(session!)) return false;
                _unitOfWork.SessionRepository.Delete(session);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Session Remove Failed : {ex}");
                return false;
            }
            
        }


        #region Helper Methods
        private bool TrainerExists(int TrainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(TrainerId) is not null;
        }

        private bool CategoryExists(int CategoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(CategoryId) is not null;
        }

        private bool IsDateValid(DateTime StartDate , DateTime EndDate)
        {
            return EndDate > StartDate;
        }

        private bool IsSessionAvailableForUpdate(Session Session)
        {
            if(Session == null) return false;
            if(Session.EndDate < DateTime.Now) return false;
            if(Session.StartDate <= DateTime.Now) return false;
            var hasActiveBookings = _unitOfWork.SessionRepository.GetCountOfBookedSlots(Session.Id) > 0;    
            if(hasActiveBookings) return false;
            return true;

        }

        private bool IsSessionAvailableForDelete(Session Session)
        {
            if (Session == null) return false;
            
            if (Session.StartDate <= DateTime.Now && Session.EndDate >= DateTime.Now) return false;
            if (Session.StartDate > DateTime.Now) return false;
            var hasActiveBookings = _unitOfWork.SessionRepository.GetCountOfBookedSlots(Session.Id) > 0;
            if (hasActiveBookings) return false;
            return true;

        }

        #endregion
    }
}
