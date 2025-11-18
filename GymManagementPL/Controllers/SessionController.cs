using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public ActionResult Index()
        {
            var Sessions = _sessionService.GetAllSessions();
            return View(Sessions);
        }

        public ActionResult Details(int Id)
        {
            if (Id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }
            var Session = _sessionService.GetSessionDetails(Id);
            if (Session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Session);
        }

        public ActionResult Create()
        {
            LoadDropDowns();
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateSessionViewModel CreatedSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDowns();
                return View(CreatedSession);
            }

            var result = _sessionService.CreateSession(CreatedSession);
            if (result)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Session Failed To Create";
                LoadDropDowns();
                return View(CreatedSession);
            }



        }

        public ActionResult Edit(int Id)
        {
            if(Id <= 0) return RedirectToAction(nameof(Index));

            var Session = _sessionService.GetSessionToUpdate(Id);
            if(Session is null)
            {
                TempData["ErrorMessage"] = "Session Can Not Be Updated";
                return RedirectToAction(nameof(Index));
            }
            LoadDropDowns();
            return View(Session);
        }

        [HttpPost]  
        public ActionResult Edit([FromRoute]int Id , UpdateSessionViewModel UpdatedSession)
        {
            if (!ModelState.IsValid) return View(UpdatedSession);

            var result = _sessionService.UpdateSession(Id, UpdatedSession);
            if (result)
                TempData["SuccessMessage"] = "Session Updated Successfully";
            
            else
                TempData["ErrorMessage"] = "Session Failed To Update";

            return RedirectToAction(nameof(Index));

        }

        public ActionResult Delete(int Id)
        {
            if (Id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }
            var Session = _sessionService.GetSessionDetails(Id);
            if (Session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.SessionId = Id;
            ViewBag.Description = Session.Description;
            ViewBag.StartDate = Session.StartDate;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int Id)
        {
            var result = _sessionService.RemoveSession(Id);
            if (result) 
                TempData["SuccessMessage"] = "Session Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Session Failed To Delete";
            return RedirectToAction(nameof(Index));
        }

        #region HelperMethods
        public void LoadDropDowns()
        {
            var Categories = _sessionService.GetCategories();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");

            var Trainers = _sessionService.GetTrainers();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
        }

        #endregion
    }
}
