﻿using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ParkSquare.BuildScreen.Web.Services.AzureDevOps;

namespace ParkSquare.BuildScreen.Web.Services
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _httpClient;

        public HttpClientFactory(IConfig config)
        {
            _httpClient = CreateInstance(config);
        }

        public HttpClient CreateClient()
        {
            return _httpClient;
        }

        private static HttpClient CreateInstance(IConfig config)
        {
            var client = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
            });

            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($":{config.AuthToken}")));

            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds);

            return client;
        }
    }
}
