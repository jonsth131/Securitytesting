using System.Threading;
using NUnit.Framework;
using NZap;
using OpenQA.Selenium;
using Securitytesting.Factories;
using Securitytesting.Helpers;
using Securitytesting.Pages;
using Securitytesting.Settings;

namespace Securitytesting
{
    [TestFixture]
    public class Securitytests
    {
        private IWebDriver _driver;
        private IZapClient _client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _client = new ZapClient(ProxySettings.Proxy, ProxySettings.ProxyPort);
            _client.Core.DeleteAllAlerts(ZapSettings.ApiKey);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Thread.Sleep(5000);
            var reportResponse = _client.Core.GetHtmlReport(ZapSettings.ApiKey);
            FileWriterHelper.WriteFile(@"c:\temp\zap\", reportResponse.ReportData);
            _client.Core.DeleteAllAlerts(ZapSettings.ApiKey);
        }

        [SetUp]
        public void SetUp()
        {
            _driver = DriverFactory.CreateWebDriver();
            _client.HttpSessions.CreateEmptySession(ZapSettings.ApiKey, ZapSettings.Target);
        }

        [TearDown]
        public void TearDown()
        {
            var activeSession = _client.HttpSessions.GetActiveSession(ZapSettings.Target);
            _client.HttpSessions.RemoveSession(ZapSettings.ApiKey, ZapSettings.Target, activeSession.Value);
            _driver.Dispose();
        }

        [Test]
        public void TestMethod1()
        {
            _client.Pscan.EnableAllScanners(ZapSettings.ApiKey);
            _driver.Navigate().GoToUrl(ZapSettings.Target);
            AlertHelper.PrintAlertsToConsole(_client.Core.GetAlerts(ZapSettings.Target));
        }

        [Test]
        public void VulnerabilityScanBeforeLogin()
        {
            _driver.Navigate().GoToUrl(ZapSettings.Target);
            var apiResponse = ScanHelper.StartSpider(_client);
            ScanHelper.WaitForSpiderToComplete(_client, apiResponse);
            apiResponse = ScanHelper.StartAscan(_client);
            ScanHelper.WaitForAscanToComplete(_client, apiResponse);
            AlertHelper.PrintAlertsToConsole(_client.Core.GetAlerts(ZapSettings.Target));
        }

        [Test]
        public void VulnerabilityScanAfterLogin()
        {
            _driver.Navigate().GoToUrl(ZapSettings.Target + "insecure/public/Login.jsp");
            var loginPage = PageObjectFactory.GetInitializedPageObject<LoginPage>(_driver);
            loginPage.SendKeysToLoginField("admin");
            loginPage.SendKeysToPasswordField("secret");
            loginPage.PressLoginButton();
            var apiResponse = ScanHelper.StartSpider(_client);
            ScanHelper.WaitForSpiderToComplete(_client, apiResponse);
            apiResponse = ScanHelper.StartAscan(_client);
            ScanHelper.WaitForAscanToComplete(_client, apiResponse);
            AlertHelper.PrintAlertsToConsole(_client.Core.GetAlerts(ZapSettings.Target));
        }


    }
}
