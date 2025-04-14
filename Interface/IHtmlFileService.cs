using Microsoft.AspNetCore.Http;
using YourNamespace.Models;

namespace YourNamespace.Services
{
    public interface IHtmlFileService
    {
        List<RepoCoverageFile> GetAllRepoCoverageFiles();

        Task<bool> AddRepoAsync(string repoName, IFormFile htmlFile);
        bool DeleteRepo(string repoName);
    }

}
