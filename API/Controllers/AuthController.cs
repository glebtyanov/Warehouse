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
        public async Task<IActionResult> Register(WorkerAddingDTO registerRequest)
        {
            logger.LogInformation("Registering new worker");
            if (!await authService.Register(registerRequest))
            {
                logger.LogWarning("Email is already taken");
                return BadRequest("Email is taken");
            }

            logger.LogInformation("Worker registered successfully");
            return Ok("Successfully registered");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(WorkerLoginDTO loginRequest)
        {
            logger.LogInformation("Processing login request");
            var loginResponse = await authService.Login(loginRequest);

            if (loginResponse.Length < 50)
            {
                logger.LogWarning("Email or password is incorrect");
                return BadRequest("Email or password is incorrect");
            }

            logger.LogInformation("Login successful");
            return Ok(loginResponse);
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword(WorkerLoginDTO changePasswordRequest)
        {
            logger.LogInformation("Processing change password request");
            var isPasswordChanged = await authService.ChangePassword(changePasswordRequest);

            if (!isPasswordChanged)
            {
                logger.LogWarning("Invalid email or new password");
                return BadRequest("Provide valid email and a new password");
            }

            logger.LogInformation("Password changed successfully");
            return Ok("Password successfully changed");
        }
    }
}
