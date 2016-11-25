using System;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using Securitytesting.Enums;
using Securitytesting.Helpers;

namespace Securitytesting.Factories
{
    public static class DriverFactory
    {
        public static IWebDriver CreateWebDriver()
        {
            var proxyHost = AppSettingsHelper.ReadString(AppSettings.Proxy);
            var proxyPort = AppSettingsHelper.ReadInt(AppSettings.ProxyPort);
            var proxyString = $"{proxyHost}:{proxyPort}";

            var proxy = new Proxy
            {
                HttpProxy = proxyString,
                FtpProxy = proxyString,
                SslProxy = proxyString
            };

            var phantomJsOptions = new PhantomJSOptions();
            phantomJsOptions.AddAdditionalCapability(CapabilityType.Proxy, proxy);

            var driver = new PhantomJSDriver(phantomJsOptions);
            driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 30));

            return driver;
        }
    }
}
