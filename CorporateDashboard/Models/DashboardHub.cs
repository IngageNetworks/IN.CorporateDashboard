using CorporateDashboard.Utils;
using SignalR.Hubs;

namespace CorporateDashboard.Models
{
    public class DashboardHub : Hub
    {
        static string _lastMessage = string.Empty;

        public void BroadcastMessage(string message, bool force)
        {
            if (!_lastMessage.Equals(message))
            {
                Clients.updateMessage(message);
                _lastMessage = message;
            }
        }

        public void ShowAlert(string message)
        {
            Clients.showAlert(message);
        }

        public void HideAlert(string message)
        {
            Clients.hideAlert(message);
        }

        public void ShowMessage(string message)
        {
            Clients.showMessage(message);
        }

        public void HideMessage(string message)
        {
            Clients.hideMessage(message);
        }
        
        public void ShowWeather(string message)
        {
            Clients.showWeather(message);
        }

        public void HideWeather(string message)
        {
            Clients.hideWeather(message);
        }
        
        public void ResendLastMessage()
        {
            Caller.updateMessage(_lastMessage);
        }

        public static ApiResponse GetNewsItemResponse()
        {
            var apiRequest = new ApiRequest(
                method: HttpMethod.GET,
                url: string.Format("{0}?parentid={1}&order=descending&offset=0&limit={2}", Constants.BlogPostsUrl, 9, 6),
                apiKey: Constants.ApiKey,
                accessToken: Constants.AccessToken);

            var apiResponse = new ApiResponse(apiRequest);

            return apiResponse;
        }

        public static ApiResponse GetEventsResponse()
        {
            var apiRequest = new ApiRequest(
                method: HttpMethod.GET,
                url: string.Format("{0}?&order=ascending&offset=0&limit={1}", Constants.EventsUrl, 6),
                apiKey: Constants.ApiKey,
                accessToken: Constants.AccessToken);

            var apiResponse = new ApiResponse(apiRequest);

            return apiResponse;
        }

        public void ResendDashboard()
        {
            var apiResponse = GetNewsItemResponse();
            var eventResponse = GetEventsResponse();

            var response = HtmlCleaner.GetFixedBlogPostsJson(apiResponse, eventResponse);

            if (Caller == null)
                Clients.updateJson(response);
            else
                Caller.updateJson(response);
        }
    }
}