using Microsoft.AspNetCore.Mvc;
using TinaFicha.Ports.In.Publish;

namespace TinaFicha.Adpaters.Publish;

[Route("api/[controller]")]
[ApiController]
public class PublishController(
    PublishVideoOnPlatforms publishVideoOnPlatforms
): ControllerBase 
{

    [HttpPost("", Name = "PostVideo")]
    public string Publish()
    {
        var command = new PublishVideoCommand(
            // fill with post request
        );
        publishVideoOnPlatforms.Publish(command);
        return "ok";
    }
}