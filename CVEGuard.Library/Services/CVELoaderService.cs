using CVEGuard.Library.Contracts;
using CVEGuard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CVEGuard.Library.Services
{
    public class CVELoaderService : ICVELoaderService
    {
        public async Task ExecuteTaskAsync(Action<List<Vulnerability>> save,int count =0)
        {
            var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(2)
            };
            var jsonString = await httpClient.GetStringAsync($"https://services.nvd.nist.gov/rest/json/cves/2.0/?startIndex={count}");

            // Deserialize JSON into a dictionary (modify based on actual structure)
            var jsonData = JsonSerializer.Deserialize<VulnerabilityResponse>(jsonString);
            save(jsonData.Vulnerabilities);
        }
    }
}
