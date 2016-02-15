using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using NZap;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Securitytesting
{
    public class Securitytests
    {
        private const string Proxy = "localhost";
        private const int ProxyPort = 8080;
        private const string Target = "http://localhost/";
        private const string ApiKey = "b56i96opec68mts9ob05pdpigg";

        private FirefoxDriver _driver;
        private IZapClient _client;

        [SetUp]
        public void SetUp()
        {
            var proxy = new Proxy
            {
                HttpProxy = "localhost:8080",
                FtpProxy = "localhost:8080",
                SslProxy = "localhost:8080"
            };
            var desiredCapabilities = new DesiredCapabilities();
            desiredCapabilities.SetCapability(CapabilityType.Proxy, proxy);
            _driver = new FirefoxDriver(desiredCapabilities);
            _driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 30));
            _client = new ZapClient(Proxy, ProxyPort);
            var parameters = new Dictionary<string, string> { { "apikey", ApiKey }, { "site", Target } };
            _client.CallApi("httpSessions", "action", "createEmptySession", parameters);
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Dispose();
            //_client.Dispose();
        }

        [Test]
        public void TestMethod1()
        {
            _client.Pscan.EnableAllScanners(ApiKey);
            _driver.Navigate().GoToUrl(Target);
            PrintAlertsToConsole();
        }

        [Test]
        public void VulnerabilityScanBeforeLogin()
        {
            _driver.Navigate().GoToUrl(Target);
            var parameters = new Dictionary<string, string> { { "apikey", ApiKey }, { "url", Target }, { "maxChildren", "5" }, { "recurse", "5" } };
            var apiResponse = _client.CallApi("spider", "action", "scan", parameters);
            var value = apiResponse.Value;
            var complete = 0;
            while (complete < 100)
            {
                var params2 = new Dictionary<string, string> { { "scanId", value } };
                var callApi = _client.CallApi("spider", "view", "status", params2);
                complete = int.Parse(callApi.Value);
                Thread.Sleep(1000);
            }
            PrintAlertsToConsole();
        }

        [Test]
        public void VulnerabilityScanAfterLogin()
        {
            _driver.Navigate().GoToUrl(Target);
            _driver.FindElementByName("username").SendKeys("admin");
            _driver.FindElementByName("password").SendKeys("password");
            _driver.FindElementByName("Login").Click();
            var parameters = new Dictionary<string, string> { { "apikey", ApiKey }, { "url", Target }, {"recurse", "5"} };
            var apiResponse = _client.CallApi("ascan", "action", "scan", parameters);
            var value = apiResponse.Value;
            var complete = 0;
            while (complete < 100)
            {
                var params2 = new Dictionary<string, string> { { "scanId", value } };
                var callApi = _client.CallApi("ascan", "view", "status", params2);
                complete = Int32.Parse(callApi.Value);
                Thread.Sleep(1000);
            }
            PrintAlertsToConsole();
        }

        private void PrintAlertsToConsole()
        {
            var alerts = _client.GetAlerts(Target);
            foreach (var alert in alerts)
            {
                Console.WriteLine(alert.Alert
                    + Environment.NewLine
                    + alert.Cweid
                    + Environment.NewLine
                    + alert.Url
                    + Environment.NewLine
                    + alert.Wascid
                    + Environment.NewLine
                    + alert.Evidence
                    + Environment.NewLine
                    + alert.Param
                    + Environment.NewLine
                );
            }
        }
    }
}
