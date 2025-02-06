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
                this.Run(command.video).Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("ERROR: " + e.Message);
                }
            }
    }

    private async Task Run(Video video)
    {
        // Compte TinaFicha
        UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets // OAUTH2 ~ API TOKEN 
                    {
                        ClientId = EnvReader.GetStringValue("CLIENT_ID"), // tina ficha
                        ClientSecret = EnvReader.GetStringValue("CLIENT_SECRET"),
                    },
                    new[] { YouTubeService.Scope.YoutubeUpload }, // Scopes
                    "user", // ??? "The user identifier"
                    CancellationToken.None // "The cancellation token for cancelling an operation"
                );

        // Create the service Youtube.
        var service = new YouTubeService(new BaseClientService.Initializer(){
            HttpClientInitializer = credential,
            ApplicationName = "tina-ficha-youtube"
        });

        var yb_video = new Google.Apis.YouTube.v3.Data.Video();
        yb_video.Snippet.Title = "Test Title";
        yb_video.Snippet.Description = "Test Video Description";
        yb_video.Status = new Google.Apis.YouTube.v3.Data.VideoStatus
        {
            PrivacyStatus = "public"
        };

        var videosInsertRequest = service.Videos.Insert(yb_video, "snippet,status", video.Stream, "video/*");
        videosInsertRequest.ProgressChanged += videosInsertRequest_ProgressChanged;
        videosInsertRequest.ResponseReceived += videosInsertRequest_ResponseReceived;

        await videosInsertRequest.UploadAsync();

    }

    void videosInsertRequest_ProgressChanged(Google.Apis.Upload.IUploadProgress progress)
    {
      switch (progress.Status)
      {
        case Google.Apis.Upload.UploadStatus.Uploading:
          Console.WriteLine("{0} bytes sent.", progress.BytesSent);
          break;

        case Google.Apis.Upload.UploadStatus.Failed:
          Console.WriteLine("An error prevented the upload from completing.\n{0}", progress.Exception);
          break;
      }
    }

    void videosInsertRequest_ResponseReceived(Google.Apis.YouTube.v3.Data.Video video)
    {
      Console.WriteLine("Video id '{0}' was successfully uploaded.", video.Id);
    }
}