using LiquidationDashboard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiquidationDashboard.Services
{
    public interface IStorageService
    {
        Task Add(Storage storage);
        Task<Storage> GetStorage(string address);
        Task<IEnumerable<Storage>> GetStorages();
    }
}