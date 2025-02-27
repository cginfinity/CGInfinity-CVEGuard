using CVEGuard.Model;

namespace CVEGuard.Library.Contracts
{
    public interface ICVEService
    {
        Task<List<Cve>> GetAllAsync();
        Task<string> TodaysCVESummaryAsync(Uri uri, string modal);
    }
}