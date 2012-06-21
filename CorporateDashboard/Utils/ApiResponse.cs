using System;
using System.IO;
using System.Net;
using System.Text;

namespace CorporateDashboard.Utils
{
    public class ApiResponse
    {
        public WebHeaderCollection Headers { get; set; }
        public string Body { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public int StatusCodeValue
        {
            get { return (int)StatusCode; }
        }

        public ApiResponse(ApiRequest request)
        {
            try
            {
                var response = GetResponse(() => { return request.GetResponse(); });
            }
            catch (WebException ex)
            {
                var response = GetResponse(() => { return ex.Response; });
            }
        }

        private HttpWebResponse GetResponse(Func<WebResponse> responseFunction)
        {
            var response = (HttpWebResponse)responseFunction();
            StatusCode = response.StatusCode;
            Stream responseStream = response.GetResponseStream();
            Headers = response.Headers;

            var streamReader = new StreamReader(responseStream, Encoding.UTF8);
            Body = streamReader.ReadToEnd();
            response.Close();
            streamReader.Close();

            return response;
        }
    }
}