using SmartStudyPlanner.Application.DTOs;

namespace SmartStudyPlanner.Application.Interfaces
{
    public interface IDashboardService
    {
        DashboardDto GetDashboard(int userId);
    }
}
