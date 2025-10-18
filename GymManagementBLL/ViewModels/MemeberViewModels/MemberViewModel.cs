using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemeberViewModels
{
    internal class MemberViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Photo { get; set; }
        public string Gender { get; set; } = null!;
        public string Phone { get; set;} = null!;

        public string? DateOfBirth { get; set; }
        public string? MemberPlanStartDate { get; set; }
        public string? MemberPlanEndDate { get; set; }
        public string? Address { get; set; }

        public string? PlanName { get; set; }



    }
}
