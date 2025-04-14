using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace HtmlViewerApp.Controllers
{
    public class UploadController : Controller
    {
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadFile(IFormFile htmlFile)
        {
            if (htmlFile != null && Path.GetExtension(htmlFile.FileName).ToLower() == ".html")
            {
                if (!Directory.Exists(_uploadPath))
                    Directory.CreateDirectory(_uploadPath);

                var fileName = Path.GetRandomFileName() + ".html";
                var filePath = Path.Combine(_uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    htmlFile.CopyTo(stream);
                }

                return RedirectToAction("Display", new { fileName });
            }

            TempData["Error"] = "Invalid file. Please upload a .html file.";
            return RedirectToAction("Index");
        }

        public IActionResult Display(string fileName)
        {
            ViewBag.FileName = fileName;
            return View();
        }
    }
}
