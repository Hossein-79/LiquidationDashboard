using LiquidationDashboard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiquidationDashboard.Services
{
    public interface ISymbolService
    {
        Task Add(string name);
        Task<Symbol> GetSymbol(string name);
        Task<IEnumerable<Symbol>> GetSymbols();
    }
}