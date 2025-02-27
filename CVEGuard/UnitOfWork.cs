using CVEGuard.Library;
using CVEGuard.Library.Contracts;
using CVEGuard.Model;
using Microsoft.EntityFrameworkCore;

namespace CVEGuard
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly CVEGuardDbContext _context;
        private ICVERepository CVERepository { get; }


        public UnitOfWork(
            CVEGuardDbContext context, 
            ICVERepository CVERepository

            ) {
            _context = context;
            this.CVERepository = CVERepository;
        }

        public async Task AddVulnerabilityAsync(Vulnerability vulnerability)
        {
            await _context.Vulnerabilities.AddAsync(vulnerability);
        }
        public async Task<int> CountVulnerabilitiesAsync(Vulnerability vulnerability)
        {
            return await _context.Vulnerabilities.CountAsync();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<List<Cve>> GetAllCveAsync()
        {
            return await _context.Cves.ToListAsync();
        }

        public async Task<int> CountVulnerabilitiesAsync()
        {
            return await _context.Cves.CountAsync();
        }

        public async Task<List<Cve>> CurrentCVEsAsync()
        {
            var maxDate = await MaxCVEPublishDateAsync();
            return await _context.Cves
                .Include(cve => cve.Descriptions) // Ensure 'Description' is a valid navigation property
                .Where(cve => cve.Published > maxDate.AddDays(-1))
                .ToListAsync();
        }

        public async Task<DateTime> MaxCVEPublishDateAsync()
        {
            return await _context.Cves.MaxAsync(s=>s.Published);
        }
    }
}
