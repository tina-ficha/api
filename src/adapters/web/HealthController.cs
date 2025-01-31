using Microsoft.AspNetCore.Mvc;

namespace TinaFicha.Adpaters.Web;

[Route("api/[controller]")] // api/health
[ApiController]
public class HealthController : ControllerBase
{

    [HttpGet("", Name = "GetHealth")] // GET /api/health
    public string GetHealth()
    {
        return "Healthy";
    }
}