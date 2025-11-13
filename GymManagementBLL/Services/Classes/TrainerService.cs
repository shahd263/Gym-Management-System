using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public bool CreatedTrainer(CreateTrainerViewModel Trainer)
        {
            try  //  in Create , Update , Delete Must use Try|Catch
            {
                if (EmailExists(Trainer.Email) || PhoneExists(Trainer.Phone)) return false;

                var trainer = _mapper.Map<Trainer>(Trainer);

                _unitOfWork.GetRepository<Trainer>().Add(trainer);
                return _unitOfWork.SaveChanges() > 0;


            }
            catch
            {
                return false;
            }
        }
    
        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (!trainers.Any()) return [];
            return _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);

        }

        public TrainerViewModel? GetTrainerDetails(int TrainerId)
        {
            var Trainer = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (Trainer is null) return null;
            return _mapper.Map<TrainerViewModel>(Trainer);
        }

        public UpdateTrainerViewModel? GetTrainerToUpdate(int Id)
        {
            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
            var trainer = TrainerRepo.GetById(Id);
            if (trainer is null) return null;

            return _mapper.Map<UpdateTrainerViewModel>(trainer);


        }

        public bool UpdateTrainer(int Id, UpdateTrainerViewModel UpdatedTrainer)
        {
            try
            {
                var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
                
                
                var EmailExists = TrainerRepo.GetAll(X => X.Email == UpdatedTrainer.Email && X.Id != Id).Any();
                var PhoneExists = TrainerRepo.GetAll(X => X.Phone == UpdatedTrainer.Phone && X.Id != Id).Any();

                if(EmailExists || PhoneExists) return false;

                var trainer = TrainerRepo.GetById(Id);
                if (trainer is null) return false;

                _mapper.Map(UpdatedTrainer, trainer);
                TrainerRepo.Update(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }

        }

        public bool DeleteTrainer(int Id)
        {
            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
            var trainer = TrainerRepo.GetById(Id);
            if(trainer is null) return false;

            var FutureSessions = _unitOfWork.GetRepository<Session>()
                .GetAll(X => X.TrainerId == Id && X.StartDate > DateTime.Now).Any();
            if (FutureSessions) return false;

            TrainerRepo.Delete(trainer);
            return _unitOfWork.SaveChanges() > 0;
            


        }
               
        private bool EmailExists(string Email)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(X => X.Email == Email).Any();
        }

        private bool PhoneExists(string Phone)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(X => X.Phone == Phone).Any();
        }
    }
}
