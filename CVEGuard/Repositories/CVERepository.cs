using CVEGuard.Library.Contracts;
using CVEGuard.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVEGuard.Library.Contracts
{
    public class CVERepository : ICVERepository
    {
        private readonly CVEGuardDbContext _context;

        public CVERepository(CVEGuardDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Cve user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Cve>> GetAllAsync()
        {
            return await _context.Cves.ToListAsync();
        }

        public async Task<Cve?> GetByIdAsync(int id)
        {
            return await _context.Cves.FindAsync(id);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Cves.CountAsync();
        }
    }
}
