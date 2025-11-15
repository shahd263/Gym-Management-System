using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GymManagementBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool EditedPlan(int PlanId, EditPlanViewModel UpdatedPlan)
        {
            var PlanRepo = _unitOfWork.GetRepository<Plan>();
            var plan = PlanRepo.GetById(PlanId);
            if (plan is null || HasActiveMemberPlans(PlanId)) return false;

            try
            {

                _mapper.Map(UpdatedPlan, plan);
                plan.UpdatedAt = DateTime.Now;

                PlanRepo.Update(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }


        }

        public IEnumerable<PlanViewModel> GetAllPlans()
        {
           
            var Plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (!Plans.Any() || Plans is null) return [];

            return _mapper.Map<IEnumerable<PlanViewModel>>(Plans);
        }

        public EditPlanViewModel? GetEditPlanViewModel(int PlanId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null || plan.IsActive || (HasActiveMemberPlans(PlanId) )) return null;
            return _mapper.Map<EditPlanViewModel>(plan);

        }

        private bool HasActiveMemberPlans(int PlanId)
        {
           

            return _unitOfWork.GetRepository<MemberPlan>().GetAll(X=> X.PlanId == PlanId && X.Status == "Active").Any();
            
            
        }
        public PlanViewModel? GetPlanDetails(int PlanId)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan == null) return null;
            return _mapper.Map<PlanViewModel>(Plan);   

        }

        public bool ToggleStatus(int PlanId)
        {
            var PlanRepo = _unitOfWork.GetRepository<Plan>();
            var plan = PlanRepo.GetById(PlanId);
            if (plan is null || HasActiveMemberPlans(PlanId) )return false;
            try
            {
                if (plan.IsActive)
                    plan.IsActive = false;
                else
                    plan.IsActive = true;
                plan.UpdatedAt = DateTime.Now;

                PlanRepo.Update(plan);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch
            {
                return false;
            }

        }
    }
}
