using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;

namespace CorporateDashboard.Utils
{
    public class ApiRequest
    {
        public string Accept { get; set; }
        public string AccessToken { get; set; }
        public string ApiKey { get; set; }
        public string ContentType { get; set; }
        public string Body { get; set; }
        public HttpMethod Method { get; set; }
        public WebHeaderCollection Headers { get; set; }
        public Uri Uri { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public ApiRequest(HttpMethod method, string url, string apiKey, string accessToken = null, string accept = null, string contentType = null, string body = null, object headers = null, string username = null, string password = null)
        {
            Method = method;
            Uri = new Uri(url);
            ApiKey = apiKey;
            AccessToken = accessToken;
            Accept = accept;
            ContentType = contentType;
            Body = body;
            Headers = new WebHeaderCollection();
            Username = username;
            Password = password;

            var headerDictionary = MakeDictionary(headers);

            foreach (var entry in headerDictionary)
                Headers.Add(entry.Key, entry.Value);
        }

        public WebResponse GetResponse()
        {
            var request = (HttpWebRequest)WebRequest.Create(Uri);

            request.Method = Enum.GetName(typeof(HttpMethod), Method);
            request.Headers = Headers;

            if (!string.IsNullOrWhiteSpace(ApiKey))
                request.Headers["Api-Key"] = ApiKey;

            if (!string.IsNullOrWhiteSpace(AccessToken))
                request.Headers["X-STS-AccessToken"] = AccessToken;

            if (!string.IsNullOrWhiteSpace(Accept))
                request.Accept = Accept;

            if (!string.IsNullOrWhiteSpace(ContentType))
                request.ContentType = ContentType;

            if (!string.IsNullOrWhiteSpace(Body))
            {
                request.ContentLength = Body.Length;

                var requestStream = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
                requestStream.Write(Body);
                requestStream.Close();
            }

            if (!string.IsNullOrWhiteSpace(Username))
                SetBasicAuthHeader(request, Username, Password);

            return request.GetResponse();
        }

        private IDictionary<string, string> MakeDictionary(object withProperties)
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();
            var properties = TypeDescriptor.GetProperties(withProperties);
            foreach (PropertyDescriptor property in properties)
            {
                dic.Add(property.Name, property.GetValue(withProperties) as string);
            }
            return dic;
        }

        public void SetBasicAuthHeader(WebRequest request, String username, String password)
        {
            var authInfo = username + ":" + password;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;
        }
    }
}