using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CorporateDashboard.Models;
using CorporateDashboard.Utils;
using SignalR;
using SignalR.Hubs;

namespace CorporateDashboard.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Admin(string message)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<DashboardHub>();
            context.Clients.updateMessage(message);

            return View();
        }

        [AllowAnonymous]
        public ActionResult CreateBlogPost()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult CreateEvent()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult JsonCreateBlogPost(BlogPost model)
        {
            if (ModelState.IsValid)
            {
                var imageHtml = HtmlCleaner.GetFirstImgNodeHtml(model.Body);
                var imageUrl = HtmlCleaner.GetFirstImgNodeSrc(model.Body);

                if (string.IsNullOrWhiteSpace(model.Title))
                {
                    model.Title = HtmlCleaner.GetFirstH1NodeText(model.Body);
                }

                model.Title = model.Title.CondenseWhiteSpaceRuns();
                var firstPass = HtmlCleaner.RemoveNodes(model.Body, "//div[@class='quote']");
                var cleanHtml = HtmlCleaner.CleanHtml(firstPass, new string []{}, new string[]{}, new[] { "script", "a", "em" }).Replace("&nbsp;", " ");
                cleanHtml = cleanHtml.CondenseWhiteSpaceRuns();
                model.Body = "<div>" + imageHtml + cleanHtml.TrimTextToMaxLength(270) + "</div>";

                model.Title = model.Title.TrimTextToMaxLength(97);
                model.Title = model.Title.ReplaceInvalidCharacters();
                model.Body = model.Body.ReplaceInvalidCharacters();
                model.ImageUrl = imageUrl;

                var body = string.Format(
                    "<CreateBlogPostRequest><BlogPost><Author>{0}</Author><BlogId>{1}</BlogId><Body><![CDATA[{2}]]></Body><Status>Published</Status><Title>{3}</Title></BlogPost></CreateBlogPostRequest>",
                    Constants.AdminUserId,
                    Constants.BlogId,
                    model.Body,
                    model.Title);

                var apiRequest = new ApiRequest(
                    method: HttpMethod.POST,
                    url: Constants.BlogPostsUrl,
                    apiKey: Constants.ApiKey,
                    accessToken: Constants.AccessToken,
                    accept: Constants.ContentTypeJson,
                    contentType: Constants.ContentTypeXml,
                    body: body);

                var apiResponse = new ApiResponse(apiRequest);
                var responseMessage = apiResponse.Body.DeserializeJson<ResponseMessage>();
                responseMessage.StatusCode = apiResponse.StatusCodeValue;

                UpdateClients();
                
                return Json(responseMessage);
            }

            // If we got this far, something failed
            return Json(new { errors = GetErrorsFromModelState() });
        }

        private void UpdateClients()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<DashboardHub>();
            var newsItemResponse = DashboardHub.GetNewsItemResponse();
            var eventResponse = DashboardHub.GetEventsResponse();
            var response = HtmlCleaner.GetFixedBlogPostsJson(newsItemResponse, eventResponse);
            context.Clients.updateJson(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult JsonCreateEvent(Event model)
        {
            if (ModelState.IsValid)
            {
                DateTime startDate;

                if (!DateTime.TryParse(model.Date, out startDate))
                    startDate = DateTime.Now;

                var endDate = startDate.AddHours(1);

                string body = string.Format(
                    "<CreateEventRequest><Event><Author>{0}</Author><Description><![CDATA[<p>{1}</p>]]></Description><EndDate><DateTime>{2:yyyy-MM-ddTHH:mm:ss}</DateTime><OffsetMinutes>-240</OffsetMinutes></EndDate><Location><![CDATA[<p>{3}</p>]]></Location><ParentId>{4}</ParentId><Recurrence>None</Recurrence><StartDate><DateTime>{5:yyyy-MM-ddTHH:mm:ss}</DateTime><OffsetMinutes>-240</OffsetMinutes></StartDate><Status>Published</Status><Title>{6}</Title><Url>http://www.ingagenetworks.com/</Url></Event></CreateEventRequest>",
                    Constants.AdminUserId,
                    model.Title,
                    endDate,
                    model.Location,
                    Constants.RootContainerId,
                    startDate,
                    model.Title);

                var apiRequest = new ApiRequest(
                    method: HttpMethod.POST,
                    url: Constants.EventsUrl,
                    apiKey: Constants.ApiKey,
                    accessToken: Constants.AccessToken,
                    contentType: Constants.ContentTypeXml,
                    body: body);

                var apiResponse = new ApiResponse(apiRequest);
                var responseMessage = new ResponseMessage { Message = apiResponse.Body, StatusCode = apiResponse.StatusCodeValue };
                
                UpdateClients();

                return Json(responseMessage);
            }

            // If we got this far, something failed
            return Json(new { errors = GetErrorsFromModelState() });
        }

        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }
    }
}
