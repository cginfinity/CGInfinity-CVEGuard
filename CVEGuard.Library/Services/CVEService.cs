using CVEGuard.Library.Contracts;
using CVEGuard.Model;
using System.Reflection;
using System.Text;

namespace CVEGuard.Library.Services
{
    public class CVEService : ICVEService
    {
        private const int perpage = 1;
        private readonly IUnitOfWork _uow;

        public CVEService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<Cve>> GetAllAsync()
        {
            return await _uow.GetAllCveAsync();
        }
        public async Task<string> TodaysCVESummaryAsync(Uri uri, string modal)
        {
            // Retrieve CVE descriptions
            var cves = await _uow.CurrentCVEsAsync();
            var cvesMaxDate = await _uow.MaxCVEPublishDateAsync();
            var descriptions = cves.Where(cve=>cve.VulnStatus!= "Rejected").SelectMany(cve => cve.Descriptions.Select(desc => desc.Value)).ToList();

            // Split the descriptions into pages of 10
            var pages = descriptions
                        .Select((desc, index) => new { desc, index })
                        .GroupBy(x => x.index / perpage)
                        .Select(g => g.Select(x => x.desc).ToList())
                        .ToList();

            // Process each page to get its summary
            List<string> pageSummaries = new List<string>();
            int totalPages = pages.Count;
            for (int i = 0; i < totalPages; i++)
            {
                string batchPrompt = $"This description contains details about a product, service, system or hardware and a description of the issue. " +
                                     "Your task is to extract and output a list of the affected item names and the vulnerabilities identified in each in English.\r\n" +
                                     "Input:\r\n" + string.Join("\r\n", pages[i]) + "\r\n";
                var batchSummary = await OllamaClient.Post(batchPrompt, uri, modal);
                pageSummaries.Add(batchSummary);
            }

            // Consolidate all page summaries into one deduplicated list.
            string consolidatedPrompt = "Given the following lists of affected items and thier vulnerabilities. " +
                                        "Produce a deduplicated list with all similar Items Grouped and in alphabetical order along with a summary in english of the identified vulnerabilities for each along with its implications.\r\n" +
                                        "Input:\r\n" + string.Join("\r\n", pageSummaries) + "\r\n";
            var consolidatedSummary = await OllamaClient.Post(consolidatedPrompt, uri, modal);

            // Format the final HTML using the provided template
            using Stream stream = this.GetType().Assembly.GetManifestResourceStream("CVEGuard.Library.Services.htmltemplate.html");
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string htmlTemplate = reader.ReadToEnd();
            string formattedHtml = await OllamaClient.Post(
                $"Using this template:\r\n{htmlTemplate}\r\n" +
                $"Lay out the following items as an HTML page, using the date {cvesMaxDate.ToShortDateString()} in the header.\r\n" +
                $"Items:\r\n{consolidatedSummary}\r\n" +
                $"Full CVE List:\r\n{string.Join("\r\n", descriptions)}\r\n", uri, modal);

            // Optionally, perform any additional HTML adjustments.
            formattedHtml = formattedHtml.Replace("</html>", $"<h3>Full CVE List for {cvesMaxDate.ToShortDateString()}</h3><p>{string.Join("</p><p>", descriptions)}</p></html>");
            return formattedHtml.RemoveBeforeHtmlTag().RemoveAfterHtmlTag();
        }

    }
}
