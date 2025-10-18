using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class MemberPlan : BaseEntity
    {

        //StartDate
        public DateTime EndDate { get; set; }

        //ReadOnly Computed Property bt return el status Active wla la 3shan mtaresh a7sbha kol mara 

        public string Status { get 
            {
                if (EndDate > DateTime.Now)
                    return "Active";
                else
                    return "Expired";
            } 
        }

        public int PlanId { get; set; }
        public Plan Plan { get; set; }
        public int MemberId { get; set; }

        public Member Member { get; set; }

    }
}
