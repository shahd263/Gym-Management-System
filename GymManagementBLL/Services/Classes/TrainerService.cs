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
    internal class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public bool CreatedTrainer(CreateTrainerViewModel Trainer)
        {
            try  //  in Create , Update , Delete Must use Try|Catch
            {
                if (EmailExists(Trainer.Email) || PhoneExists(Trainer.Phone)) return false;

                var trainer = new Trainer()
                {
                    Name = Trainer.Name,
                    Email = Trainer.Email,
                    Phone = Trainer.Phone,
                    Gender = Trainer.Gender,
                    DateOfBirth = Trainer.DateOfBirth,
                    Address = new Address()
                    {
                        BuildingNumber = Trainer.BuildingNumber,
                        Street = Trainer.Street,
                        City = Trainer.City,
                    },
                    Specialties = Trainer.Specialties,
                    CreatedAt = DateTime.Now
                    
                  

                };

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
            return trainers.Select(X => new TrainerViewModel()
            {
                Id = X.Id,
                Name = X.Name,
                Email = X.Email,
                Phone = X.Phone,
                Specialization = X.Specialties.ToString()

            });

        }

        public TrainerViewModel? GetTrainerDetails(int TrainerId)
        {
            var Trainer = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (Trainer is null) return null;
            return new TrainerViewModel()
            {
                Name = Trainer.Name,
                Email = Trainer.Email,
                Phone = Trainer.Phone,
                DateOfBirth = Trainer.DateOfBirth.ToShortDateString(),
                Specialization = Trainer.Specialties.ToString(),
                Address = $"{Trainer.Address.BuildingNumber} - {Trainer.Address.Street} - {Trainer.Address.City}"


            };
        }

        public UpdateTrainerViewModel? GetTrainerToUpdate(int Id)
        {
            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
            var trainer = TrainerRepo.GetById(Id);
            if (trainer is null) return null;

            return new UpdateTrainerViewModel()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City,
                Specialties = trainer.Specialties
                
            };


        }

        public bool UpdateTrainer(int Id, UpdateTrainerViewModel UpdatedTrainer)
        {
            try
            {
                var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
                var trainer = TrainerRepo.GetById(Id);
                if (EmailExists(UpdatedTrainer.Email) || PhoneExists(UpdatedTrainer.Phone)) return false;

                (trainer.Email, trainer.Specialties, trainer.Address.BuildingNumber, trainer.Address.Street, trainer.Address.City)
                    = (UpdatedTrainer.Email, UpdatedTrainer.Specialties, UpdatedTrainer.BuildingNumber, UpdatedTrainer.Street, UpdatedTrainer.City);

                TrainerRepo?.Update(trainer);
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
            return _unitOfWork?.SaveChanges() > 0;
            


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
