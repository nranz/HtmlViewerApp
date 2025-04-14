using Microsoft.AspNetCore.Mvc;
using YourNamespace.Models;
using YourNamespace.Services;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HtmlFilesController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHtmlFileService _fileService;

        public HtmlFilesController(IWebHostEnvironment env, IHtmlFileService fileService)
        {
            _env = env;
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult GetHtmlFiles()
        {
            var htmlDir = Path.Combine(_env.WebRootPath, "html-files");

            if (!Directory.Exists(htmlDir))
            {
                return NotFound("HTML files directory not found.");
            }

            var files = Directory.GetFiles(htmlDir, "*.html")
                                 .Select(Path.GetFileName)
                                 .ToList();

            return Ok(files);
        }

        [HttpGet("coverage")]
        public IActionResult GetAllCoverageFiles()
        {
            var files = _fileService.GetAllRepoCoverageFiles();
            return Ok(files);
        }

        [HttpPut("addRepo")]
        public async Task<IActionResult> AddRepoAsync([FromForm] RepoUploadRequest request)
        {
            var result = await _fileService.AddRepoAsync(request.RepoName, request.HtmlFile);
            return result ? Ok("Repo Added") : BadRequest("Invalid input or Repo already exists.");
        }

        [HttpDelete("deleteRepo/{repoName}")]
        public IActionResult DeleteRepo(string repoName)
        {
            var result = _fileService.DeleteRepo(repoName);

            return result ? Ok("Repo Deleted") : BadRequest("Invalid RepoName or Repo does not exist.");
        }


    }
}
