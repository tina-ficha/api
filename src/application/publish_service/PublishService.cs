using TinaFicha.Ports.In.Publish;

namespace TinaFicha.Application.Publish;

public class PublishService : PublishVideoOnPlatforms
{
    public void Publish(PublishVideoCommand command)
    {
        // get auth for youtube account (token)
        // 
        throw new NotImplementedException();
    }
}