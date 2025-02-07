using TinaFicha.Ports.In.Publish;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using dotenv.net;
using dotenv.net.Utilities;


namespace TinaFicha.Application.Publish;

public class PublishService : PublishVideoOnPlatforms
{
    public void Publish(PublishVideoCommand command)
    {
        DotEnv.Load();
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
        var store = new Google.Apis.Util.Store.FileDataStore("Youtube.Upload");
        Console.WriteLine("Youtube Store at : ", store.FolderPath);
        UserCredential credential;
        
        credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            new ClientSecrets // OAUTH2 ~ API TOKEN 
            {
                ClientId = EnvReader.GetStringValue("CLIENT_ID"), // tina ficha
                ClientSecret = EnvReader.GetStringValue("CLIENT_SECRET"),
            },
            // This OAuth 2.0 access scope allows an application to upload files to the
            // authenticated user's YouTube channel, but doesn't allow other types of access.
            new[] { 
                YouTubeService.Scope.Youtube, 
                YouTubeService.Scope.YoutubeForceSsl, 
                YouTubeService.Scope.YoutubeUpload
                },
            "user", // id dans le store
            CancellationToken.None,
            store
        );

        // Create the service Youtube.
        var service = new YouTubeService(new BaseClientService.Initializer(){
            HttpClientInitializer = credential,
            ApplicationName = "tina-ficha-youtube"
        });

        var yb_video = new Google.Apis.YouTube.v3.Data.Video();
        yb_video.Snippet = new Google.Apis.YouTube.v3.Data.VideoSnippet();
        yb_video.Snippet.Title = "Test Title";
        yb_video.Snippet.Description = "Test Video Description";
        yb_video.Status = new Google.Apis.YouTube.v3.Data.VideoStatus();
        yb_video.Status.PrivacyStatus = "public";


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