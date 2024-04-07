using LibraryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAdminSettingsService adminSettingsService) : ControllerBase
    {
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAdminPin([FromBody] string pin)
        {
            var isValid = await adminSettingsService.AuthenticateAdminPin(pin);
            return Ok(new { IsValid = isValid });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAdminPin([FromBody] string newPin)
        {
            await adminSettingsService.UpdateAdminPin(newPin);
            return NoContent();
        }
    }
}
