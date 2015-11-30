using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OWASPZAPDotNetAPI;

namespace Securitytesting
{
    public class Securitytests
    {
        private const string Proxy = "localhost";
        private const int ProxyPort = 8080;
        private const string Target = "http://localhost";

        private FirefoxDriver driver;
        private ClientApi client;
        private static IApiResponse _apiResponse;

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
            driver = new FirefoxDriver(desiredCapabilities);
            driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 30));
            client = new ClientApi(Proxy, ProxyPort);
        }

        [Test]
        public void TestMethod1()
        {
            driver.Navigate().GoToUrl(Target);
            PrintAlertsToConsole();
            driver.Dispose();
        }

        private void PrintAlertsToConsole()
        {
            var alerts = client.GetAlerts(Target, 0, 0);
            foreach (var alert in alerts)
            {
                Console.WriteLine(alert.AlertMessage
                    + Environment.NewLine
                    + alert.CWEId
                    + Environment.NewLine
                    + alert.Url
                    + Environment.NewLine
                    + alert.WASCId
                    + Environment.NewLine
                    + alert.Evidence
                    + Environment.NewLine
                    + alert.Parameter
                    + Environment.NewLine
                );
            }
        }
    }
}
