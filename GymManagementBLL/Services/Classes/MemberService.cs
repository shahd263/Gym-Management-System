using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemeberViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        //Ask CLR for creating object from Service
        //CLR Will Inject Address of Object in Constructor
        public MemberService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

       

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var Members = _unitOfWork.GetRepository<Member>().GetAll();
            if (Members is null || !Members.Any() ) return Enumerable.Empty<MemberViewModel>(); //Empty Enumerable can be like this '[]' too  

           

            return _mapper.Map<IEnumerable<MemberViewModel>>(Members);

        }


        public bool CreateMember(CreateMemberViewModel CreatedMember)
        {
            try  //  in Create , Update , Delete Must use Try|Catch
            {
                if (EmailExists(CreatedMember.Email) || PhoneExists(CreatedMember.Phone)) return false;
                
                var member = _mapper.Map<Member>(CreatedMember);


                _unitOfWork.GetRepository<Member>().Add(member);
                return _unitOfWork.SaveChanges() > 0;


            }
            catch (Exception)
            {
                return false;
            }
        }

        public MemberViewModel? GetMemberDetails(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member is null ) return null;

            var ViewModel = _mapper.Map<MemberViewModel>(member);


            var ActiveMemberPlan = _unitOfWork.GetRepository<MemberPlan>().GetAll(X => X.MemberId == MemberId && X.Status == "Active")
                                                    .FirstOrDefault();
            if(ActiveMemberPlan is not null )
            {
                ViewModel.MemberPlanStartDate = ActiveMemberPlan.CreatedAt.ToShortDateString();
                ViewModel.MemberPlanEndDate = ActiveMemberPlan.EndDate.ToShortDateString();
                
                var plan = _unitOfWork.GetRepository<Plan>().GetById(ActiveMemberPlan.PlanId); //!!!!
                ViewModel.PlanName = plan?.Name;

            }
            return ViewModel;

        }


        public HealthRecordViewModel? GetMemberHealthDetails(int MemberId)
        {
            var healthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);
            if(healthRecord is null) return null;

            return _mapper.Map<HealthRecordViewModel>(healthRecord);
        }

        public MemberUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if(member is null) return null;
            
            return _mapper.Map<MemberUpdateViewModel>(member);
        }

        public bool UpdateMember(int Id, MemberUpdateViewModel UpdatedMember)
        {
            try
            {

                var EmailExists = _unitOfWork.GetRepository<Member>().GetAll(X => X.Email == UpdatedMember.Email && X.Id != Id);
                var PhoneExists = _unitOfWork.GetRepository<Member>().GetAll(X => X.Phone == UpdatedMember.Phone && X.Id != Id);

                if(EmailExists.Any() || PhoneExists.Any()) return false;
                var member = _unitOfWork.GetRepository<Member>().GetById(Id);
                if (member is null) return false;

                _mapper.Map(UpdatedMember, member);

               _unitOfWork.GetRepository<Member>().Update(member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool RemoveMember(int MemberId)
        {
            var MemberRepo = _unitOfWork.GetRepository<Member>();
            var member = MemberRepo.GetById(MemberId);
            if (member is null) return false;

            var SessionIds = _unitOfWork.GetRepository<SessionBooking>()
                .GetAll(X => X.MemberId == MemberId).Select(X=>X.SessionId);
            //This because navigational Property Does not Load Data so if we try to get data direct Exception will be trown
            var HasFutureSessions = _unitOfWork.SessionRepository.GetAll(X => SessionIds.Contains(X.Id) && X.StartDate > DateTime.Now).Any();

            if (HasFutureSessions) return false;

            var MemberPlanRepo = _unitOfWork.GetRepository<MemberPlan>();

            var memberPlans = MemberPlanRepo.GetAll(X => X.MemberId == MemberId);

            try
            {
                if (memberPlans.Any())
                {
                    foreach (var memberPlan in memberPlans)
                       MemberPlanRepo.Delete(memberPlan);
                }

                MemberRepo.Delete(member);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch
            {
                return false ;
            }


            
        }



        private bool EmailExists(string Email)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Email == Email).Any();
        }

        private bool PhoneExists(string Phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Phone == Phone).Any();
        }
    }
}
