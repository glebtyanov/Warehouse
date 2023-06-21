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

        public AuthController(AuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(WorkerAddingDTO registerRequest)
        {
            if (!await authService.Register(registerRequest))
            {
                return BadRequest("Email is taken");
            }

            return Ok("Successfully registered");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(WorkerLoginDTO loginRequest)
        {
            var loginResponse = await authService.Login(loginRequest);

            // i know its terrible 
            if (loginResponse.Length<50)
                return BadRequest("Email or password is incorrect");

            return Ok(loginResponse);
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword(WorkerLoginDTO changePasswordRequest)
        {
            var isPasswordChanged = await authService.ChangePassword(changePasswordRequest);

            if (!isPasswordChanged)
                return BadRequest("Provide valid email and a new password");

            return Ok("Password successfully changed");
        }
    }
}
