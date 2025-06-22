using DiagNexusApi.Data;
using DiagNexusApi.Model;
using DiagNexusApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiagNexusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly DatabaseService _dbService;
        private readonly IReportServices _reportServices;

        public ReportController(IReportServices reportServices)
        {
            _reportServices = reportServices;
            _dbService = new DatabaseService();
        }

        // GET: api/Report/{key}
        [HttpGet("{key}")]
        public async Task<IActionResult> FetchReportAsync(string key)
        {
            //this will download report from S3 bucket using the provided key
            var stream = await _reportServices.FetchReportAsync(key);
            if (stream == null)
                return NotFound();
            return File(stream, "application/octet-stream");
        }

        // POST: api/Report/upload
        [HttpPost]
        [Route("upload")]
        public async Task<ActionResult<string>> UploadReportAsync([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is required.");

            var uploadedKey = await _reportServices.UploadReportAsync(file);
            return Ok(uploadedKey);
        }

        // GET: api/User/reports
        [HttpGet("reports")]
        public async Task<ActionResult<List<Report>>> GetAllReportsAsync()
        {
            var reports = await _dbService.GetAllReportsAsync();
            return Ok(reports);
        }

        // GET: api/User/report/{reportId}
        [HttpGet("report/{reportId}")]
        public async Task<ActionResult<Report>> GetReportByIdAsync(int reportId)
        {
            var report = await _dbService.GetReportByIdAsync(reportId);
            if (report == null)
                return NotFound();
            return Ok(report);
        }

        //POST: api/User/report
        [HttpPost("report")]
        public async Task<ActionResult<int>> AddReportAsync([FromBody] Report report)
        {
            if (report == null)
                return BadRequest("Report data is required.");

            var newReportId = await _dbService.AddReportAsync(report);
            return Ok(newReportId);
        }

        // GET: api/User/{userId}/reports
        [HttpGet("{userId}/reports")]
        public async Task<ActionResult<List<Report>>> GetReportsByUserAsync(int userId)
        {
            var reports = await _dbService.GetReportsByUserAsync(userId);
            return Ok(reports);
        }
    }
}