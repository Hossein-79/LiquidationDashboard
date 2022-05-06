using LiquidationDashboard.Data;
using LiquidationDashboard.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiquidationDashboard.Services
{
    public class StorageService : IStorageService
    {
        private readonly LiquidationContext _context;

        public StorageService(LiquidationContext walletManagerContext)
        {
            _context = walletManagerContext;
        }

        public async Task Add(Storage storage)
        {
            var st = await _context.Storages.Where(u => u.Address == storage.Address).FirstOrDefaultAsync();

            if (st is null)
            {
                await _context.AddAsync(storage);
                Console.WriteLine("Add");
            }
            else
            {
                st.MaxBorrow = storage.MaxBorrow;
                st.UserBorrow = storage.UserBorrow;
                _context.Update(st);
                Console.WriteLine("Update");
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Storage> Search(string address) =>
            await _context.Storages.Where(u => u.Address == address).FirstOrDefaultAsync();

        public async Task<IEnumerable<Storage>> GetStorages() =>
            await _context.Storages.ToListAsync();

        public async Task<Storage> GetStorage(string address) =>
            await _context.Storages.Where(u => u.Address == address).FirstOrDefaultAsync();
    }
}
