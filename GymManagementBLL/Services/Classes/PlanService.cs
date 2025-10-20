using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GymManagementBLL.Services.Classes
{
    internal class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool EditedPlan(int PlanId, EditPlanViewModel UpdatedPlan)
        {
            var PlanRepo = _unitOfWork.GetRepository<Plan>();
            var plan = PlanRepo.GetById(PlanId);
            if (plan is null || HasActiveMemberPlans(PlanId)) return false;

            try
            {

                plan.Name = UpdatedPlan.Name;
                plan.Description = UpdatedPlan.Description;
                plan.DurationDays = UpdatedPlan.DurationDays;
                plan.Price = UpdatedPlan.Price;
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

            return Plans.Select(X => new PlanViewModel()
            {
                Id  =X.Id,
                Name = X.Name,
                Description = X.Description,
                DurationDays = X.DurationDays,
                Price = X.Price,
                IsActive = X.IsActive
            });
        }

        public EditPlanViewModel? GetEditPlanViewModel(int PlanId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null || !plan.IsActive || HasActiveMemberPlans(PlanId)) return null;
            return new EditPlanViewModel()
            {
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
            };
            
        }

        private bool HasActiveMemberPlans(int PlanId)
        {
            return _unitOfWork.GetRepository<MemberPlan>().GetAll(X=> X.Id == PlanId && X.Status == "Active").Any();
        }
        public PlanViewModel? GetPlanDetails(int PlanId)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan == null) return null;
            return new PlanViewModel()
            {
                Id = Plan.Id,
                Name = Plan.Name,
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                Price = Plan.Price,
                IsActive = Plan.IsActive
            };
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
