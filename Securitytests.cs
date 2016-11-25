using NUnit.Framework;
using Securitytesting.Factories;
using Securitytesting.Helpers;
using Securitytesting.Pages;

namespace Securitytesting
{
    [TestFixture]
    public class Securitytests : SecuritytestsBase
    {
        [Test]
        public void VulnerabilityScanBeforeLogin()
        {
            Client.Pscan.EnableAllScanners(ApiKey);

            Driver.Navigate().GoToUrl(Target);

            var apiResponse = ScanHelper.StartSpider(Client);
            ScanHelper.WaitForTaskToComplete(apiResponse, Client.Spider.GetStatus);

            apiResponse = ScanHelper.StartAscan(Client);
            ScanHelper.WaitForTaskToComplete(apiResponse, Client.Ascan.GetStatus);

            AlertHelper.PrintAlertsToConsole(Client.Core.GetAlerts(Target));
        }

        [Test]
        public void VulnerabilityScanAfterLogin()
        {
            Client.Pscan.EnableAllScanners(ApiKey);

            Driver.Navigate().GoToUrl(Target + "insecure/public/Login.jsp");

            var loginPage = PageObjectFactory.GetInitializedPageObject<LoginPage>(Driver);
            loginPage.SendKeysToLoginField("admin");
            loginPage.SendKeysToPasswordField("secret");
            loginPage.PressLoginButton();

            var apiResponse = ScanHelper.StartSpider(Client);
            ScanHelper.WaitForTaskToComplete(apiResponse, Client.Spider.GetStatus);

            apiResponse = ScanHelper.StartAscan(Client);
            ScanHelper.WaitForTaskToComplete(apiResponse, Client.Ascan.GetStatus);

            AlertHelper.PrintAlertsToConsole(Client.Core.GetAlerts(Target));
        }
    }
}
