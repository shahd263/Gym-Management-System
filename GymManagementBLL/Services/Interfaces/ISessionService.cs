using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();

        SessionViewModel? GetSessionDetails(int sessionId);
        bool CreateSession(CreateSessionViewModel CreatedSession);

        UpdateSessionViewModel? GetSessionToUpdate(int SessionId);
        bool UpdateSession(int SessionId, UpdateSessionViewModel UpdatedSession);

        bool RemoveSession(int SessionId);

        IEnumerable<TrainerSelectViewModel> GetTrainers();
        IEnumerable<CategorySelectViewModel> GetCategories();


    }
}
