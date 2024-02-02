using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NIHR.StudyManagement.API.Controllers
{
    [Route("api/v1/[controller]")]
    public class HomeController : ApiControllerBasex
    {
        [HttpGet]
        public IActionResult Welcome()
        {
            return Ok("Welcome to the Study Management API.");
        }

        [HttpGet]
        [Authorize]
        [Route("authenticated")]
        public IActionResult WelcomeAuthenticated()
        {
            return Ok("Welcome to the Study Management API - you're authenticated.");
        }
    }
}
