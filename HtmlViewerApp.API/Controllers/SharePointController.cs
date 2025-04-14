using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using YourNamespace.Services;

[ApiController]
[Route("api/[controller]")]
public class SharePointTestController : ControllerBase
{
    private readonly ICodeCoverageReportService _reportService;
    private readonly GraphServiceClient _graphClient;


    public SharePointTestController(ICodeCoverageReportService reportService, GraphServiceClient graphClient)
    {
        _reportService = reportService;
        _graphClient = graphClient;

    }

    [HttpGet("fetch-reports")]
    public async Task<IActionResult> FetchReports()
    {
        var reports = await _reportService.FetchReportsAsync();
        return Ok(reports);
    }



}
