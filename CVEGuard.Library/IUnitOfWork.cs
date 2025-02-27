using CVEGuard.Library.Contracts;
using CVEGuard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVEGuard.Library
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();
        Task AddVulnerabilityAsync(Vulnerability vulnerability);
        Task<List<Cve>> GetAllCveAsync();
        Task<int> CountVulnerabilitiesAsync();
        Task<List<Cve>> CurrentCVEsAsync();
        Task<DateTime> MaxCVEPublishDateAsync();
    }
}
