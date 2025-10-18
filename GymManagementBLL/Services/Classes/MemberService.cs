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
    internal class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;


        //Ask CLR for creating object from Service
        //CLR Will Inject Address of Object in Constructor
        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var Members = _unitOfWork.GetRepository<Member>().GetAll();
            if (Members is null || !Members.Any() ) return Enumerable.Empty<MemberViewModel>(); //Empty Enumerable can be like this '[]' too  

            #region Projection Way

            var MembersViewModel = Members.Select(X => new MemberViewModel()
            { 
                Id = X.Id,
                Name = X.Name,
                Email = X.Email,
                Photo = X.Photo,    
                Phone = X.Phone,
                Gender = X.Gender.ToString(),
            });

            #endregion

            return MembersViewModel;

        }


        public bool CreateMember(CreateMemberViewModel CreatedMember)
        {
            try  //  in Create , Update , Delete Must use Try|Catch
            {
                if (EmailExists(CreatedMember.Email) || PhoneExists(CreatedMember.Phone)) return false;

                var member = new Member()
                {
                    Name = CreatedMember.Name,
                    Email = CreatedMember.Email,
                    Phone = CreatedMember.Phone,
                    Gender = CreatedMember.Gender,
                    DateOfBirth = CreatedMember.DateOfBirth,
                    Address = new Address()
                    {
                        BuildingNumber = CreatedMember.BuildingNumber,
                        Street = CreatedMember.Street,
                        City = CreatedMember.City,
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = CreatedMember.HealthRecordViewModel.Height,
                        Weight = CreatedMember.HealthRecordViewModel.Weight,
                        BloodType = CreatedMember.HealthRecordViewModel.BloodType,
                        Note= CreatedMember.HealthRecordViewModel.Note,
                    }

                };

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

            var ViewModel = new MemberViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}"
            };


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

            var ViewModel = new HealthRecordViewModel()
            {
               Height = healthRecord.Height,
               Weight = healthRecord.Weight,
               BloodType = healthRecord.BloodType,
               Note = healthRecord.Note,
            };
            return ViewModel;
        }

        public MemberUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if(member is null) return null;
            var ViewModel = new MemberUpdateViewModel()
            {
                Name = member.Name,
                Photo = member.Photo,
                Email = member.Email,
                Phone = member.Phone,
                BuildingNumber = member.Address.BuildingNumber,
                Street = member.Address.Street,
                City = member.Address.City,
            };
            return ViewModel;
        }

        public bool UpdateMember(int Id, MemberUpdateViewModel UpdatedMember)
        {
            try
            {
                if (EmailExists(UpdatedMember.Email) || PhoneExists(UpdatedMember.Phone)) return false;

                var member = _unitOfWork.GetRepository<Member>().GetById(Id);
                if (member is null) return false;

                member.Email = UpdatedMember.Email;
                member.Phone = UpdatedMember.Phone;
                member.Address.BuildingNumber = UpdatedMember.BuildingNumber;
                member.Address.Street= UpdatedMember.Street;
                member.Address.City = UpdatedMember.City;
                member.UpdatedAt = DateTime.Now;


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

            var ActiveBookings = _unitOfWork.GetRepository<SessionBooking>()
                .GetAll(X => X.MemberId == MemberId && X.Session.StartDate > DateTime.Now).Any();
            if (ActiveBookings) return false;

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
