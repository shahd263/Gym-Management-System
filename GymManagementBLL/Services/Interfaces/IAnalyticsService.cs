﻿using GymManagementBLL.ViewModels.AnalyticsVeiwModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IAnalyticsService
    {
        AnalyticsVeiwModel GetAnalyticsData();
    }
}
