using Microsoft.AspNetCore.Mvc;
using OTP_System_Server.Models;
using OTP_System_Server.Services;

namespace OTP_System.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] CredentialsModel credentials)
        {
            try
            {
                var user = _userService.RegisterUser(credentials.Username, credentials.Password);
                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
            }
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] CredentialsModel credentials)
        {
            try
            {
                var user = _userService.Authenticate(credentials.Username, credentials.Password);
                if (user == null)
                {
                    return Unauthorized("Invalid username or password");
                }

                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
            }
        }

        [HttpPost("validateotp")]
        public IActionResult ValidateOtp([FromBody] OtpValiationModel otpValidation)
        {
            try
            {
                var isValid = _userService.ValidateOtp(otpValidation.Username, otpValidation.Otp);
                if (!isValid)
                    return BadRequest("Invalid OTP");

                return Ok("OTP is valid");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
            }
        }
    }
}
