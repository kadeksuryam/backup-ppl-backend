using App.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class TestAdminController : ControllerBase
    {
        [Authorize(Role = "Admin")]
        [HttpGet("test_admin")]
        public IActionResult Register()
        {
            return Ok(new { message = "Admin Ops Successful" });
        }
    }
}
