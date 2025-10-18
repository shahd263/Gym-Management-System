using GymManagementBLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    internal interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlans();
        PlanViewModel? GetPlanDetails(int PlanId);
        EditPlanViewModel? GetEditPlanViewModel(int PlanId);
        bool EditedPlan(int PlanId,EditPlanViewModel UpdatedPlan);
        bool ToggleStatus (int PlanId);

    }
}
