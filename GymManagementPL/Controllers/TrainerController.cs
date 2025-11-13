using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }
        public ActionResult Index()
        {
            var Trianers = _trainerService.GetAllTrainers();
            return View(Trianers);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateTrainerViewModel CreatedTrainer)
        {
            if (!ModelState.IsValid) return View(CreatedTrainer);

            var result = _trainerService.CreatedTrainer(CreatedTrainer);
            if (result)
                TempData["SuccessMessage"] = "Trainer Created Successfully";
            else
                TempData["ErrorMessage"] = "Trainer Failed To Create";
            return RedirectToAction(nameof(Index));

        }


        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can't Be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Member = _trainerService.GetTrainerDetails(id);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Invalid Trainer";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);

        }

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can't Be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Member = _trainerService.GetTrainerToUpdate(id);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Invalid Trainer";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }


        [HttpPost]
        public ActionResult Edit([FromRoute]int id,UpdateTrainerViewModel UpdatedTrainer)
        {
            if(!ModelState.IsValid) return View(UpdatedTrainer);

            var result = _trainerService.UpdateTrainer(id, UpdatedTrainer);
            if (result)
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
            else
                TempData["ErrorMessage"] = "Trainer Failed To Update";

            
            return RedirectToAction(nameof(Index));
        } 


        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can't Be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Member = _trainerService.GetTrainerDetails(id);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Invalid Trainer";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainerId = id;
            ViewBag.TrainerName = Member.Name;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm] int id)
        {
            var result = _trainerService.DeleteTrainer(id);
            if (result)
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Trainer Failed To Delete";
            return RedirectToAction(nameof(Index));
        }
    }
}
