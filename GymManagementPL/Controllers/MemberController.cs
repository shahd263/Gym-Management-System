using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemeberViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design.Serialization;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
           _memberService = memberService;
        }



        public ActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }

        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can't Be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Invalid Member";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id can't be 0 or negative number";
                return RedirectToAction(nameof(Index));
            }

            var HealthRecord = _memberService.GetMemberHealthDetails(id);
            if (HealthRecord is null) 
            {
                TempData["ErrorMessage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index)); 
            }

            return View(HealthRecord);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel CreatedMember)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("InvalidData", "Check Data and Missing Fields");
                return View(nameof(Create), CreatedMember);
            }
            var result = _memberService.CreateMember(CreatedMember);
            if (result) TempData["SuccessMessage"] = "Member Created Successfully";
            else TempData["ErrorMessage"] = "Member Creation Failed";

            return RedirectToAction(nameof(Index));
            
        }

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can't Be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMemberToUpdate(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Invalid Member";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute] int id , MemberUpdateViewModel UpdatedMember)
        {
            if (!ModelState.IsValid) return View(UpdatedMember);

            var result = _memberService.UpdateMember(id, UpdatedMember);
            if (result)
                TempData["SuccessMessage"] = "Member Updated Successfully";
            else
                TempData["ErrorMessage"] = "Member Failed To Update";

            return RedirectToAction(nameof(Index));
            
        }

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can't Be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Invalid Member";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            ViewBag.MemberName = member.Name;
            return View();
        }

        [HttpPost]

        public ActionResult DeleteConfirmed( [FromForm]int id )
        {
            var result = _memberService.RemoveMember(id);
            if (result)
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Member Failed To Delete";
            return RedirectToAction(nameof(Index));

        }
    }
}
