using LiquidationDashboard.Models;
using System.Threading.Tasks;

namespace LiquidationDashboard.Services
{
    public interface IUserService
    {
        Task Add(User user);
        Task<User> GetUser(int useId);
        Task<User> GetUser(string name);
    }
}