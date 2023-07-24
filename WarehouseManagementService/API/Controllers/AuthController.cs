using BLL.DTO;
using BLL.DTO.Adding;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService;
        private readonly ILogger<AuthController> logger;

        public AuthController(AuthService authService, ILogger<AuthController> logger)
        {
            this.authService = authService;
            this.logger = logger;
        }

        [HttpPost("register")]
        public IActionResult Register(WorkerAddingDTO registerRequest)
        {
            logger.LogInformation("Registering new worker");
            authService.Register(registerRequest);

            logger.LogInformation("Worker registered successfully");
            return Ok("Successfully registered");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(WorkerLoginDTO loginRequest)
        {
            logger.LogInformation("Processing login request");
            var loginResponse = await authService.Login(loginRequest);

            logger.LogInformation("Login successful");

            return Ok(loginResponse);
        }

        [HttpPost("changePassword")]
        public IActionResult ChangePassword(WorkerLoginDTO changePasswordRequest)
        {
            logger.LogInformation("Processing change password request");
            authService.ChangePassword(changePasswordRequest);

            logger.LogInformation("Password changed successfully");
            return Ok("Password successfully changed");
        }
    }
}
