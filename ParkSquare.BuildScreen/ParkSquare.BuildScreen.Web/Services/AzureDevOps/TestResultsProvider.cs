﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkSquare.BuildScreen.Web.Services.AzureDevOps.Dto;

namespace ParkSquare.BuildScreen.Web.Services.AzureDevOps
{
    public class TestResultsProvider : ITestResultsProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TestResultsProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<TestResults> GetTestsForBuildAsync(string project, string buildUri)
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var requestPath = GetRequestPath(project, buildUri);
                var requestUri = new Uri(new Uri("https://dev.azure.com/parksq/"), requestPath);

                var response = await client.GetAsync(requestUri);

                if (response.IsSuccessStatusCode)
                {
                    var deserialized = await DeserializeResponseAsync(response);
                    return CalculateTestResults(deserialized.Value);
                }
            }

            return null;
        }

        private static async Task<GetTestRunsResponseDto> DeserializeResponseAsync(HttpResponseMessage response)
        {
            var deserialized = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GetTestRunsResponseDto>(deserialized);
        }

        private static string GetRequestPath(string project, string buildUri)
        {
            return $"{project}/_apis/test/runs?buildUri={buildUri}&$top=5000&api-version=5.0";
        }

        private static TestResults CalculateTestResults(IReadOnlyCollection<TestRunDto> testResults)
        {
            return new TestResults
            {
                TotalTests = testResults.Sum(x => x.TotalTests),
                PassedTests = testResults.Sum(x => x.PassedTests)
            };
        }
    }
}
