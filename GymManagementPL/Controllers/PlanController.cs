using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }
        public ActionResult Index()
        {
            var Plans = _planService.GetAllPlans();
            return View(Plans);
        }

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));

            }
            var plan = _planService.GetPlanDetails(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetEditPlanViewModel(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Can Not Be Updated";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }


        [HttpPost]
        public ActionResult Edit([FromRoute] int id, EditPlanViewModel updatedPlan)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Please, Check Data Validation");
                return View(updatedPlan);

            }

            var result = _planService.EditedPlan(id, updatedPlan);
            if (result)
                TempData["SuccessMessage"] = "Plan Updated Successfully";
            else
                TempData["ErrorMessage"] = "Failed To Update Plan";

            return RedirectToAction(nameof(Index));



        }


        [HttpPost]

        public ActionResult Activate([FromRoute] int id)
        {
            var result = _planService.ToggleStatus(id);
            if (result)
                TempData["SuccessMessage"] = "Plan Status Changed Successfully";
            else
                TempData["ErrorMessage"] = "Failed To Change Plan Status";
            return RedirectToAction(nameof(Index));
        }
    }
    }