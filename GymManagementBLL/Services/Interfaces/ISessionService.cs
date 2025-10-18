using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    internal interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();

        SessionViewModel? GetSessionDetails(int sessionId);
        bool CreateSession(CreateSessionViewModel CreatedSession);

        UpdateSessionViewModel? GetSessionToUpdate(int SessionId);
        bool UpdateSession(int SessionId, UpdateSessionViewModel UpdatedSession);

        bool RemoveSession(int SessionId);

    }
}
