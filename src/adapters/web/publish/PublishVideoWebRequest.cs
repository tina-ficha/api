using Microsoft.AspNetCore.Mvc;

namespace TinaFicha.Adpaters.Web.Publish;

public class PublishVideoWebRequest {
    [FromForm(Name="video")]
    public required IFormFile Video { get; set; }

    [FromForm(Name="platforms")]
    public required List<string> Platforms { get; set; }
}