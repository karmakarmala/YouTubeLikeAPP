using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.IO;

namespace Google.Apis.YouTube.Samples
{
    /// <summary>
    /// YouTube Data API v3 sample: upload a video.
    /// Relies on the Google APIs Client Library for .NET, v1.7.0 or higher.
    /// See https://developers.google.com/api-client-library/dotnet/get_started
    /// </summary>
    internal class UploadVideo
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("YouTube Data API: Get no. of Youtube Video Likes");
            Console.WriteLine("==============================");

            try
            {

            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
        }

        private async Task<YouTubeService> GetYouTubeService(string userEmail)
        {
            UserCredential credential;
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows for read-only access to the authenticated
                    // user's account, but not other types of account access.
                    new[] { YouTubeService.Scope.YoutubeReadonly },
                    userEmail,
                    CancellationToken.None,
                    new FileDataStore(GetType().ToString())
                );
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = GetType().ToString()
            });

            return youtubeService;
        }


        private async Task<ulong?> GetVideoLikes(YouTubeService service)
        {
            ulong? likecount = 0;

            var videolistRequest = service.Videos.List("statistic");
            videolistRequest.Id = "VIDEO_ID"; // Replace with video id


            // Call the video.list method to retrieve results matching the specified query term.
            var videolistResponse = await videolistRequest.ExecuteAsync();

            foreach (var Videos in videolistResponse.Items)
            {
                likecount = Videos.Statistics.LikeCount;
            }

            Console.WriteLine("The number of likes for Video is {0}", likecount);
            return likecount;
        }


    }
}