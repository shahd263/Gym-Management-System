using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Member :GymUser
    {
        //JoinDate
        public string? Photo { get; set; }
        public HealthRecord HealthRecord { get; set; }

        public ICollection<MemberPlan> MemberPlans { get; set; }

        public ICollection<SessionBooking> SessionBookings { get; set; }
    }
}
