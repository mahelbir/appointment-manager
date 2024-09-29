using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class AuthController: BaseController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login()
    {
        return Ok();
    }
    
    [HttpPut("logout")]
    public async Task<IActionResult> Logout()
    {
        return Ok();
    }
    
}