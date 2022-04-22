using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiquidationDashboard.Services
{
    public interface IApiService
    {
        Task<IEnumerable<string>> GetActiveSymbols();
        Task<IEnumerable<GetStoragesDto>> GetStorages(string symbol);
    }
}