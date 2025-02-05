using TinaFicha.Ports.In.Publish;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using dotenv.net.Utilities;


namespace TinaFicha.Application.Publish;

public class PublishService : PublishVideoOnPlatforms
{
    public void Publish(PublishVideoCommand command)
    {
        // https://stackoverflow.com/questions/78801795/how-can-i-simply-use-the-youtube-api-to-upload-videos
        // Oauth identification -> service Oauth ?
        // send the vidoe to youtube api -> publishing service ?

        try
            {
                this.Run().Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("ERROR: " + e.Message);
                }
            }
    }

    private async Task Run()
    {
        UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets
                    {
                        ClientId = EnvReader.GetStringValue("CLIENT_ID"),
                        ClientSecret = EnvReader.GetStringValue("CLIENT_SECRET"),
                    },
                    new[] { YouTubeService.Scope.YoutubeUpload },
                    "user",
                    CancellationToken.None
                );

        // Create the service.
        var service = new YouTubeService(new BaseClientService.Initializer(){
            HttpClientInitializer = credential,
            ApplicationName = "tina-ficha-youtube"
        });

        //service.Videos.Insert()
        //var bookshelves = await service.Mylibrary.Bookshelves.List().ExecuteAsync();
    }
}