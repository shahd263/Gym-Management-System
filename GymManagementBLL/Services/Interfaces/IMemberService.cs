using GymManagementBLL.ViewModels.MemeberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    internal interface IMemberService
    {
        IEnumerable<MemberViewModel> GetAllMembers();
        bool CreateMember(CreateMemberViewModel CreatedMember);
        MemberViewModel? GetMemberDetails(int MemberId);
        HealthRecordViewModel? GetMemberHealthDetails(int MemberId);
        MemberUpdateViewModel? GetMemberToUpdate(int MemberId);
        bool UpdateMember(int Id ,MemberUpdateViewModel UpdatedMember);
        bool RemoveMember(int MemberId);

    }
}
