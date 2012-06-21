using CorporateDashboard.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CorporateDashboard.Tests
{
    /// <summary>
    /// These are not really unit tests.  They were just used to experiment with fuctionality.
    /// </summary>
    [TestClass]
    public class ApiTests
    {
        private const int ChildContainerId = 8;
        private const int BlogId = 9;
        private const int BlogPostId = 10;
        private string ChildContainerUrl = Constants.ContainersUrl + ChildContainerId;

        [TestMethod]
        public void Version()
        {
            var apiRequest = new ApiRequest(
                method: HttpMethod.GET,
                url: Constants.VersionUrl,
                apiKey: Constants.ApiKey);

            var apiResponse = new ApiResponse(apiRequest);

            Assert.IsNotNull(apiResponse.Body);
        }

        [TestMethod]
        public void Authenticate()
        {
            var apiRequest = new ApiRequest(
                method: HttpMethod.GET,
                url: Constants.AuthenticationUrl,
                apiKey: Constants.ApiKey,
                username: Constants.Username,
                password: Constants.Password);

            var apiResponse = new ApiResponse(apiRequest);

            Assert.IsNotNull(apiResponse.Body);
        }

        [TestMethod]
        public void ListContainers()
        {
            var apiRequest = new ApiRequest(
                method: HttpMethod.GET,
                url: Constants.ContainersUrl,
                apiKey: Constants.ApiKey,
                accessToken: Constants.AccessToken);

            var apiResponse = new ApiResponse(apiRequest);

            Assert.IsNotNull(apiResponse.Body);
        }

        [TestMethod]
        public void ReadContainer()
        {
            var apiRequest = new ApiRequest(
                method: HttpMethod.GET,
                url: Constants.RootContainerUrl,
                apiKey: Constants.ApiKey,
                accessToken: Constants.AccessToken);

            var apiResponse = new ApiResponse(apiRequest);

            Assert.IsNotNull(apiResponse.Body);
        }

        [TestMethod]
        public void CreateChildContainer()
        {
            var apiRequest = new ApiRequest(
                method: HttpMethod.POST,
                url: Constants.RootContainerUrl,
                apiKey: Constants.ApiKey,
                accessToken: Constants.AccessToken,
                contentType: Constants.ContentTypeXml,
                body: "<CreateChildContainerRequest><Container><Name>Test Child Container</Name><Status>Published</Status></Container></CreateChildContainerRequest>");

            var apiResponse = new ApiResponse(apiRequest);

            Assert.IsNotNull(apiResponse.Body);
        }

        [TestMethod]
        public void CreateBlog()
        {
            string body = string.Format(
                "<CreateBlogRequest><Blog><Author>{0}</Author><Name>Test Blog</Name><ParentId>{1}</ParentId><Status>Published</Status></Blog></CreateBlogRequest>",
                Constants.AdminUserId,
                ChildContainerId);

            var apiRequest = new ApiRequest(
                method: HttpMethod.POST,
                url: Constants.BlogsUrl,
                apiKey: Constants.ApiKey,
                accessToken: Constants.AccessToken,
                contentType: Constants.ContentTypeXml,
                body: body);

            var apiResponse = new ApiResponse(apiRequest);

            Assert.IsNotNull(apiResponse.Body);
        }

        [TestMethod]
        public void CreateBlogPost()
        {
            string body = string.Format(
                "<CreateBlogPostRequest><BlogPost><Author>{0}</Author><BlogId>{1}</BlogId><Body><![CDATA[<p>Blog post body</p>]]></Body><Status>Published</Status><Title>Test Blog Post</Title></BlogPost></CreateBlogPostRequest>",
                Constants.AdminUserId,
                BlogId);

            var apiRequest = new ApiRequest(
                method: HttpMethod.POST,
                url: Constants.BlogPostsUrl,
                apiKey: Constants.ApiKey,
                accessToken: Constants.AccessToken,
                contentType: Constants.ContentTypeXml,
                body: body);

            var apiResponse = new ApiResponse(apiRequest);

            Assert.IsNotNull(apiResponse.Body);
        }

        [TestMethod]
        public void ListBlogPosts()
        {
            var apiRequest = new ApiRequest(
                method: HttpMethod.GET,
                url: string.Format("{0}?parentid={1}&order=descending", Constants.BlogPostsUrl, BlogId),
                apiKey: Constants.ApiKey,
                accessToken: Constants.AccessToken);

            var apiResponse = new ApiResponse(apiRequest);

            Assert.IsNotNull(apiResponse.Body);
        }

        [TestMethod]
        public void CreateEvent()
        {
            string body = string.Format(
                "<CreateEventRequest><Event><Author>{0}</Author><Description><![CDATA[<p>Test Event Description</p>]]></Description><EndDate><DateTime>2012-06-01T12:00:00</DateTime><OffsetMinutes>0</OffsetMinutes></EndDate><Location><![CDATA[<p>Test Location</p>]]></Location><ParentId>{1}</ParentId><Recurrence>None</Recurrence><StartDate><DateTime>2012-06-01T11:00:00</DateTime><OffsetMinutes>0</OffsetMinutes></StartDate><Status>Published</Status><Title>Test Event</Title><Url>http://www.ingagenetworks.com/</Url></Event></CreateEventRequest>",
                Constants.AdminUserId,
                Constants.RootContainerId);

            var apiRequest = new ApiRequest(
                method: HttpMethod.POST,
                url: Constants.EventsUrl,
                apiKey: Constants.ApiKey,
                accessToken: Constants.AccessToken,
                contentType: Constants.ContentTypeXml,
                body: body);

            var apiResponse = new ApiResponse(apiRequest);

            Assert.IsNotNull(apiResponse.Body);
        }
    }
}
