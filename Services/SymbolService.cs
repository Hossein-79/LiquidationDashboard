using LiquidationDashboard.Data;
using LiquidationDashboard.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiquidationDashboard.Services
{
    public class SymbolService : ISymbolService
    {
        private readonly LiquidationContext _context;

        public SymbolService(LiquidationContext walletManagerContext)
        {
            _context = walletManagerContext;
        }

        public async Task Add(string name)
        {
            if (!_context.Symbols.Where(u => u.Name == name).Any())
            {
                var symbol = new Symbol
                {
                    Name = name,
                };
                await _context.AddAsync(symbol);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Symbol> GetSymbol(string name) =>
            await _context.Symbols.Where(u => u.Name == name).FirstOrDefaultAsync();
        

        public async Task<IEnumerable<Symbol>> GetSymbols() =>
            await _context.Symbols.ToListAsync();
    }
}
