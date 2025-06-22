using DiagNexusApi.Data;
using DiagNexusApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace DiagNexusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        public AuthController() { 
            _dbService = new DatabaseService();
        }

        private readonly DatabaseService _dbService;
        [HttpPost("login")]
        public ActionResult<bool> Login([FromBody] LoginRequest request)
        {
            // Replace this with your actual authentication logic
            bool isValid = (request.Username == "admin" && request.Password == "password");
            return _dbService.VerifyUserAsync(request.Username, request.Password).Result;
            //return Ok(isValid);
        }
        // GET: api/User
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<List<User>>> GetAllUsersAsync()
        {
            var users = await _dbService.GetAllUsersAsync();
            return Ok(users);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
