using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using AzureDevopsWork.Services.Models.Views;

namespace AzureDevopsWork.Services
{
    public class HttpService
    {

        public HttpClient IniciateHttp(Config config)
        {
            if (config.Validate())
            {
                var client = new HttpClient {BaseAddress = new Uri(config.uri)};

                var token = $"{string.Empty}:{config.access_token}";
                var encodedToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(token));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedToken);
                return client;
            }

            return null;
        }
    }
}
