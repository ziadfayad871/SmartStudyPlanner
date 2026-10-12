using SmartStudyPlanner.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Application.Interfaces
{
    public interface IStatisticsService
    {
        Task<StatisticsDto> GetStatistics(int userId);
    }
}
