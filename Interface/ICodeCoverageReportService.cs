using YourNamespace.Models;



namespace YourNamespace.Services
{
    public interface ICodeCoverageReportService
    {
        /// <summary>
        /// Fetches a list of available coverage reports (e.g., repoName + fileName).
        /// </summary>
        Task<IEnumerable<RepoCoverageFile>> FetchReportsAsync();

        /// <summary>
        /// Downloads the raw HTML content of a report for a specific repo.
        /// </summary>
        // Task<string> DownloadHtmlAsync(string repoName, string fileName);
    }
}


