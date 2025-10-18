﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemeberViewModels
{
    internal class HealthRecordViewModel
    {

        [Required(ErrorMessage = "Height Is Required")]
        [Range(0.1, 300, ErrorMessage = "Height Number Must Be Greater Than 0 and Less Than 300")]
        public decimal Height { get; set; }


        [Required(ErrorMessage = "Width Is Required")]
        [Range(0.1, 500, ErrorMessage = "Width Number Must Be Greater Than 0 and Less Than 500")]
        public decimal Weight { get; set; }


        [Required(ErrorMessage = "Boold Type Is Required")]
        [StringLength(3, ErrorMessage = "Name Must Be 3 Char or Less")]
        public string BloodType { get; set; } = null!;
        public string? Note { get; set; }


    }
}
