using Microsoft.Graph;
using Microsoft.Graph.Models;
using YourNamespace.Models;

namespace YourNamespace.Services;

public class SharePointCoverageReportService : ICodeCoverageReportService
{
    private readonly GraphServiceClient _graphClient;
    private readonly string _siteId;
    private readonly string _documentLibraryPath;

    public SharePointCoverageReportService(GraphServiceClient graphClient, IConfiguration config)
    {
        _graphClient = graphClient;
        _siteId = config["SharePoint:SiteId"] ??
                throw new InvalidOperationException("Missing SharePoint:SiteId in configuration.");
        _documentLibraryPath = config["SharePoint:CoverageFolder"] ??
                throw new InvalidOperationException("Missing SharePoint:CoverageFolder in configuration.");
    }
    public async Task<IEnumerable<RepoCoverageFile>> FetchReportsAsync()
    {
        var results = new List<RepoCoverageFile>();

        // Step 1: Get the Drive (default document library for the site)
        var drive = await _graphClient
            .Sites[_siteId]
            .Drive
            .GetAsync();

        // Step 2: Get folder for "Shared Documents/CodeCoverage"
        var folder = await _graphClient
            .Drives[drive?.Id]
            .Root
            .ItemWithPath(_documentLibraryPath) // e.g. "Shared Documents/CodeCoverage"
            .GetAsync();

        // Step 3: Get subfolders (repos)
        var repoFolders = await _graphClient
            .Drives[drive?.Id]
            .Items[folder?.Id]
            .Children
            .GetAsync();

        foreach (var repoFolder in repoFolders?.Value ?? new List<DriveItem>())
        {
            if (repoFolder.Folder == null) continue;

            var repoName = repoFolder.Name!;

            // Step 4: Get HTML files inside the repo folder
            var files = await _graphClient
                .Drives[drive?.Id]
                .Items[repoFolder.Id]
                .Children
                .GetAsync();

            foreach (var file in files?.Value ?? new List<DriveItem>())
            {
                if (file.File != null && !string.IsNullOrEmpty(file.Name)
                    && file.Name.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(new RepoCoverageFile
                    {
                        RepoName = repoName,
                        FileName = file.Name
                    });
                }
            }
        }

        return results;
    }

    // public Task<string> DownloadHtmlAsync(string repoName, string fileName) { }
}


