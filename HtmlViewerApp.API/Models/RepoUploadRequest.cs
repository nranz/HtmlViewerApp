using Microsoft.AspNetCore.Http;

namespace YourNamespace.Models
{
    public class RepoUploadRequest
    {
        public string RepoName { get; set; } = string.Empty;

        public IFormFile HtmlFile { get; set; } = default!;
    }
}
