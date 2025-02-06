using Microsoft.AspNetCore.Mvc;
using TinaFicha.Ports.In.Publish;

namespace TinaFicha.Adpaters.Web.Publish;

[Route("api/[controller]")]
[ApiController]
public class PublishController(
    PublishVideoOnPlatforms publishVideoOnPlatforms
): ControllerBase 
{

    [HttpPost("", Name = "PostVideo")]
    public void Publish(PublishVideoWebRequest request)
    {
        var command = new PublishVideoCommand(
            Video.FromStream(request.Video.OpenReadStream()),
            [.. request.Platforms]
        );
        publishVideoOnPlatforms.Publish(command);
    }
}

