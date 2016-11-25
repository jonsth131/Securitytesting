using System.Collections.Generic;
using System.Threading;
using NZap;
using NZap.Entities;
using Securitytesting.Enums;

namespace Securitytesting.Helpers
{
    public static class ScanHelper
    {
        public delegate IApiResult StatusDelegate(string value);

        public static IApiResult StartAscan(IZapClient client)
        {
            var apiKey = AppSettingsHelper.ReadString(AppSettings.ApiKey);
            var target = AppSettingsHelper.ReadString(AppSettings.Target);

            client.Ascan.EnableAllScanners(apiKey);

            var parameters = new Dictionary<string, string> { { "recurse", "5" } };
            var apiResponse = client.Ascan.Scan(apiKey, target, parameters);
            return apiResponse;
        }

        public static IApiResult StartSpider(IZapClient client)
        {
            var apiKey = AppSettingsHelper.ReadString(AppSettings.ApiKey);
            var target = AppSettingsHelper.ReadString(AppSettings.Target);

            var parameters = new Dictionary<string, string> { { "maxChildren", "5" }, { "recurse", "5" } };
            var apiResponse = client.Spider.Scan(apiKey, target, parameters);
            return apiResponse;
        }

        public static void WaitForTaskToComplete(IApiResult apiResponse, StatusDelegate getStatus)
        {
            var value = apiResponse.Value;
            var complete = 0;
            while (complete < 100)
            {
                var callApi = getStatus(value);
                complete = int.Parse(callApi.Value);
                Thread.Sleep(1000);
            }
        }
    }
}
