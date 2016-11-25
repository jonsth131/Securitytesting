using System.Threading;
using NUnit.Framework;
using NZap;
using OpenQA.Selenium;
using Securitytesting.Enums;
using Securitytesting.Factories;
using Securitytesting.Helpers;

namespace Securitytesting
{
    public class SecuritytestsBase
    {
        protected IWebDriver Driver;
        protected IZapClient Client;
        protected string Target;
        protected string ApiKey;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var proxy = AppSettingsHelper.ReadString(AppSettings.Proxy);
            var proxyPort = AppSettingsHelper.ReadInt(AppSettings.ProxyPort);
            Target = AppSettingsHelper.ReadString(AppSettings.Target);
            ApiKey = AppSettingsHelper.ReadString(AppSettings.ApiKey);

            Client = new ZapClient(proxy, proxyPort);
            Client.Core.DeleteAllAlerts(ApiKey);
            Driver = DriverFactory.CreateWebDriver();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Thread.Sleep(5000);
            var reportResponse = Client.Core.GetHtmlReport(ApiKey);
            FileWriterHelper.WriteReport(AppSettingsHelper.ReadString(AppSettings.ReportPath), reportResponse.ReportData);
            Client.Core.DeleteAllAlerts(ApiKey);
        }

        [SetUp]
        public void SetUp()
        {
            Client.HttpSessions.CreateEmptySession(ApiKey, Target);
        }

        [TearDown]
        public void TearDown()
        {
            var activeSession = Client.HttpSessions.GetActiveSession(Target);
            Client.HttpSessions.RemoveSession(ApiKey, Target, activeSession.Value);
            Driver.Dispose();
        }
    }
}
