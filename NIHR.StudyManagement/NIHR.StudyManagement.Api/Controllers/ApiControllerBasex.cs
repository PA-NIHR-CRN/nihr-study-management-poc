using Microsoft.AspNetCore.Mvc;

namespace NIHR.StudyManagement.API.Controllers
{
    [ApiController]
    public abstract class ApiControllerBasex : ControllerBase
    {
        [Route("/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error()
        {
            return Ok();
        }
    }
}
