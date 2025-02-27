using CVEGuard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVEGuard.Library.Contracts
{
    public interface ICVERepository
    {
        Task<List<Cve>> GetAllAsync();
        Task<Cve?> GetByIdAsync(int id);
        Task AddAsync(Cve user);
        Task DeleteAsync(int id);
        Task<int> CountAsync();
    }
}
