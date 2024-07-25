using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await authService.AuthenticateAsync(loginDto.Username, loginDto.Password);
            if (result == null)
            {
                return Unauthorized();
            }
            return Ok(result);
        }
        
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            var result = await authService.RefreshTokenAsync(refreshToken);
            if (result == null)
            {
                return Unauthorized();
            }
            return Ok(result);
        }
        
        [Authorize]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDto resetDto)
        {
            
            var authenticatedUsername = User.Identity?.Name;
            if (authenticatedUsername != resetDto.Username)
            {
                return Forbid();
            }
            
            var result = await authService.ResetPasswordAsync(resetDto.Username, resetDto.NewPassword);
            if (result)
            {
                return Ok(new { Message = "Password reset successfully" });
            }
            return BadRequest(new { Message = "Failed to reset password" });
        }
    }
}