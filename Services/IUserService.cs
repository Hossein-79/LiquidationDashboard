using LiquidationDashboard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiquidationDashboard.Services
{
    public interface IUserService
    {
        Task Add(User user);
        Task AddAlert(Alert alert);
        Task DeleteAlert(int alertId, int userId);
        Task<User> GetUser(int useId);
        Task<User> GetUser(string name);
        Task<IEnumerable<Alert>> GetUserAlerts(int userId);
    }
}