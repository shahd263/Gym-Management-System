﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.AnalyticsVeiwModels
{
    public class AnalyticsVeiwModel
    {
        public int TotalMembers { get; set; }
        public int ActiveMembers { get; set; }
        public int Trainers { get; set; }
        public int UpcomingSessions { get; set; }
        public int OngoingSessions { get; set; }
        public int CompletedSessions { get; set; }

    }
}
