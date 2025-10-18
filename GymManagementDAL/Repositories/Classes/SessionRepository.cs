using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepository(GymDbContext dbContext):base(dbContext)
        {
           _dbContext = dbContext;
        }

        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory()
        {
            return _dbContext.Sessions
                .Include(X => X.Trainer)
                .Include(X=> X.Category).ToList();

        }

        public int GetCountOfBookedSlots(int sessionId)
        {
            return _dbContext.SessionBookings.Count(X => X.SessionId == sessionId);
        }

        public Session? GetSessionWithTrainerAndCategory(int sessionId)
        {
            return _dbContext.Sessions
                .Include(X => X.Category)
                .Include(X => X.Trainer)
                .FirstOrDefault(X => X.Id == sessionId);

        }
    }
}
