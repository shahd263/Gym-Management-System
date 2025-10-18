using GymManagementBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    internal interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();
        TrainerViewModel? GetTrainerDetails(int TrainerId);
        bool CreatedTrainer (CreateTrainerViewModel Trainer);
        UpdateTrainerViewModel? GetTrainerToUpdate(int TrainerId);
        bool UpdateTrainer(int Id, UpdateTrainerViewModel UpdatedTrainer);
        bool DeleteTrainer(int Id);

    }
}
