using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemeberViewModels
{
    internal class CreateMemberViewModel
    {
        [Required(ErrorMessage = "Name Is Required")]
        [StringLength(50 , MinimumLength =2 , ErrorMessage = "Name Must Be From 2 to 50 Characters") ]
        [RegularExpression(@"^[a-zA-Z\s]+$" , ErrorMessage = "Name Can Contain Only  Letters And Spaces")]
        public string Name { get; set; } = null!;


        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Ivavlid Email Format")]
        [DataType(DataType.EmailAddress)] // UI Hint
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email Must Be From 5 to 100 Characters ")]
        public string Email { get; set; } = null!;
        

        [Required(ErrorMessage = "Gender Is Required")]
        public Gender Gender { get; set; }


        [Required(ErrorMessage = "Date Of Birth Is Required")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }


        [Required(ErrorMessage = "Phone Is Required")]
        [Phone(ErrorMessage = "Invalid Phone Number Format")]
        [RegularExpression (@"^(010|011|012|015)\d{8}$" , ErrorMessage = "Phone Number Must Be Egyption Phone Number")]
        [DataType (DataType.PhoneNumber)] // UI Hint
        public string Phone { get; set; } = null!;


        [Required(ErrorMessage = "Building Number Is Required")]
        [Range(1,9000 , ErrorMessage = "Building Number Must Be Between 1 and 9000")]
        public int BuildingNumber { get; set; }


        [Required(ErrorMessage = "Street Is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street Must Be From 2 to 30 Characters")]
        public string Street { get; set; } = null!;


        [Required(ErrorMessage = "City Is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "City Must Be From 2 to 30 Characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City Can Contain Only Letters And Spaces")]
        public string City { get; set; } = null!;


        [Required(ErrorMessage = "Heakth Record Is Required")]
        public HealthRecordViewModel HealthRecordViewModel { get; set; } = null!;
            



    }
}
