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
        private const string Target = "http://192.168.56.101:8080/insecure/";
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
            _client.HttpSessions.CreateEmptySession(ApiKey, Target);
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
            var parameters = new Dictionary<string, string> { { "maxChildren", "5" }, { "recurse", "5" } };
            var apiResponse = _client.Spider.Scan(ApiKey, Target, parameters);
            var value = apiResponse.Value;
            var complete = 0;
            while (complete < 100)
            {
                var callApi = _client.Spider.GetStatus(value);
                complete = int.Parse(callApi.Value);
                Thread.Sleep(1000);
            }
            PrintAlertsToConsole();
        }

        [Test]
        public void VulnerabilityScanAfterLogin()
        {
            _driver.Navigate().GoToUrl(Target + "public/Login.jsp");
            _driver.FindElementByName("login").SendKeys("admin");
            _driver.FindElementByName("pass").SendKeys("secret");
            _driver.FindElementByXPath("html/body/table/tbody/tr[2]/td[2]/center/form/table/tbody/tr[3]/td/input[2]").Click();
            var parameters = new Dictionary<string, string> { {"recurse", "5"} };
            var apiResponse = _client.Ascan.Scan(ApiKey, Target, parameters);
            var value = apiResponse.Value;
            var complete = 0;
            while (complete < 100)
            {
                var callApi = _client.Ascan.GetStatus(value);
                complete = int.Parse(callApi.Value);
                Thread.Sleep(1000);
            }
            PrintAlertsToConsole();
        }

        private void PrintAlertsToConsole()
        {
            var alerts = _client.Core.GetAlerts(Target);
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
