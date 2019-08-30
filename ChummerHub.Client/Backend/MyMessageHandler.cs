using Microsoft.Rest;
using NLog;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ChummerHub.Client.Backend
{
    public class MyMessageHandler : DelegatingHandler
    {
        private readonly Logger Log = NLog.LogManager.GetCurrentClassLogger();
        public MyMessageHandler()
        {

            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Proxy = WebRequest.DefaultWebProxy,// new WebProxy("http://localhost:8888"),
                UseProxy = true,

                Credentials = CredentialCache.DefaultCredentials
            };
            httpClientHandler.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
            httpClientHandler.PreAuthenticate = true;
            httpClientHandler.CookieContainer = new CookieContainer();
            httpClientHandler.UseDefaultCredentials = true;
            httpClientHandler.Credentials = System.Net.CredentialCache.DefaultCredentials;

            InnerHandler = httpClientHandler;
        }

        public static int requestCounter = 0;

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                int myCounter = requestCounter++;
                string msg = "Process request " + myCounter + ": " + request.RequestUri;
                Log.Debug(msg);
                // Call the inner handler.
                request.Headers.TryAddWithoutValidation("ContentType", "application/json");
                HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
                msg = "Process response " + myCounter + " (" + (((double)sw.ElapsedMilliseconds) / 1000) + "): " + response.StatusCode;
                Log.Debug(msg);
                return response;
            }
            catch (Exception e)
            {
                e.Data.Add("request", request.AsFormattedString());
                Log.Error(e);
                throw;
            }

        }
    }
}
