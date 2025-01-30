using Microsoft.AspNetCore.Mvc;

namespace TinaFicha.Adpaters.Web;

[Route("api/[controller]")]
[ApiController]
public class HealthController : ControllerBase
{

    [HttpGet]
    public string GetHealth()
    {
        return "Healthy";
    }
}