using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Text;
using YourNamespace.Models;

namespace YourNamespace.Services
{
    public class HtmlFileService : IHtmlFileService
    {

        private readonly string _htmlFileDirectory;

        public HtmlFileService(IWebHostEnvironment env)
        {
            _htmlFileDirectory = Path.Combine(env.WebRootPath, "html-files");

            if (!Directory.Exists(_htmlFileDirectory))
            {
                Directory.CreateDirectory(_htmlFileDirectory);
            }
        }

        public List<RepoCoverageFile> GetAllRepoCoverageFiles()
        {
            var result = new List<RepoCoverageFile>();

            if (!Directory.Exists(_htmlFileDirectory))
                // will return an empty list if directory does not exist.
                return result;

            var repoDirs = Directory.GetDirectories(_htmlFileDirectory);
            foreach (var repoDir in repoDirs)
            {
                var repoName = Path.GetFileName(repoDir);
                var htmlFiles = Directory.GetFiles(repoDir, "*.html");

                result.AddRange(htmlFiles.Select(file => new RepoCoverageFile
                {
                    RepoName = repoName,
                    FileName = Path.GetFileName(file)
                }));
            }

            return result;
        }


        // public async Task<bool> AddRepoAsync(string repoName, IFormFile htmlFile)
        // {
        //     if (string.IsNullOrWhiteSpace(repoName) || htmlFile == null || !htmlFile.FileName.EndsWith(".html"))
        //         return false;

        //     var repoPath = Path.Combine(_htmlFileDirectory, repoName);

        //     if (Directory.Exists(repoPath))
        //         return false;

        //     Directory.CreateDirectory(repoPath); // Create repo folder if it doesn't exist

        //     var filePath = Path.Combine(repoPath, htmlFile.FileName);

        //     using var stream = new FileStream(filePath, FileMode.Create);
        //     await htmlFile.CopyToAsync(stream);

        //     return true;
        // }
        public async Task<bool> AddRepoAsync(string repoName, IFormFile htmlFile)
        {
            if (string.IsNullOrWhiteSpace(repoName) || htmlFile == null || !htmlFile.FileName.EndsWith(".html"))
                return false;

            var repoPath = Path.Combine(_htmlFileDirectory, repoName);
            if (Directory.Exists(repoPath))
                return false;

            Directory.CreateDirectory(repoPath);

            // Read the uploaded HTML
            using var reader = new StreamReader(htmlFile.OpenReadStream());
            var content = await reader.ReadToEndAsync();

            // Inject <link> tag into <head>
            var cssLink = "<link rel=\"stylesheet\" href=\"../report.css\">";
            content = content.Replace("<head>", $"<head>\n  {cssLink}");


            // Save the modified HTML
            var filePath = Path.Combine(repoPath, htmlFile.FileName);
            await File.WriteAllTextAsync(filePath, content);

            return true;
        }


        public bool DeleteRepo(string repoName)
        {
            var repoPath = Path.Combine(_htmlFileDirectory, repoName);

            if (!Directory.Exists(repoPath)) return false;

            Directory.Delete(repoPath, true); // delete folder and its contents
            return true;
        }

    }
}
