
namespace CorporateDashboard.Utils
{
    public static class Constants
    {
        // These values need to be updated based on your authentication information.
        public const string ApiKey = "YOUR API KEY";
        // For production applications, the access token can't be hard coded since it will expire.
        // The application should get a valid access token by calling the Authentication API.
        public const string AccessToken = "YOUR ACCESS TOKEN";
        public const string Username = "YOUR USERNAME";
        public const string Password = "YOUR PASSWORD";

        // These values need to be updated based on your data.
        public const int AdminUserId = 10;
        public const int RootContainerId = 7;
        public const int BlogId = 9;
        public const int HackathonContainer = 130;

        public const string ContentTypeJson = "text/json";
        public const string ContentTypeXml = "text/xml";

        public const string VersionUrl = "https://api.ingagenetworks.com/v1/version/";
        public const string AuthenticationUrl = "https://api.ingagenetworks.com/v1/authentication/";
        public const string ContainersUrl = "http://api.ingagenetworks.com/v1/containers/";
        public const string BlogsUrl = "http://api.ingagenetworks.com/v1/blogs/";
        public const string BlogPostsUrl = "https://api.ingagenetworks.com/v1/blogs/posts";
        public const string EventsUrl = "https://api.ingagenetworks.com/v1/events/";

        public static string RootContainerUrl = ContainersUrl + RootContainerId;
    }
}