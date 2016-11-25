using System.Collections.Generic;
using System.Threading;
using NZap;
using NZap.Entities;
using Securitytesting.Settings;

namespace Securitytesting.Helpers
{
    public static class ScanHelper
    {
        public static IApiResult StartAscan(IZapClient client)
        {
            var parameters = new Dictionary<string, string> { { "recurse", "5" } };
            var apiResponse = client.Ascan.Scan(ZapSettings.ApiKey, ZapSettings.Target, parameters);
            return apiResponse;
        }

        public static IApiResult StartSpider(IZapClient client)
        {
            var parameters = new Dictionary<string, string> { { "maxChildren", "5" }, { "recurse", "5" } };
            var apiResponse = client.Spider.Scan(ZapSettings.ApiKey, ZapSettings.Target, parameters);
            return apiResponse;
        }

        public static void WaitForAscanToComplete(IZapClient client, IApiResult apiResponse)
        {
            var value = apiResponse.Value;
            var complete = 0;
            while (complete < 100)
            {
                var callApi = client.Ascan.GetStatus(value);
                complete = int.Parse(callApi.Value);
                Thread.Sleep(1000);
            }
        }

        public static void WaitForSpiderToComplete(IZapClient client, IApiResult apiResponse)
        {
            var value = apiResponse.Value;
            var complete = 0;
            while (complete < 100)
            {
                var callApi = client.Spider.GetStatus(value);
                complete = int.Parse(callApi.Value);
                Thread.Sleep(1000);
            }
        }
    }
}
