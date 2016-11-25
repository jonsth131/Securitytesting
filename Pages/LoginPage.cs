using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
#pragma warning disable 649

namespace Securitytesting.Pages
{
    public class LoginPage : PageObject
    {
        [FindsBy(How = How.XPath, Using = "/html/body/table/tbody/tr[2]/td[2]/center/form/table/tbody/tr[2]/td/input")]
        private IWebElement _loginField;

        [FindsBy(How = How.XPath, Using = "/html/body/table/tbody/tr[2]/td[2]/center/form/table/tbody/tr[2]/td/input[1]")]
        private IWebElement _passwordField;

        [FindsBy(How = How.XPath, Using = "/html/body/table/tbody/tr[2]/td[2]/center/form/table/tbody/tr[3]/td/input[2]")]
        private IWebElement _loginButton;

        public LoginPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public void SendKeysToLoginField(string s)
        {
            _loginField.SendKeys(s);
        }

        public void SendKeysToPasswordField(string s)
        {
            _passwordField.SendKeys(s);
        }

        public void PressLoginButton()
        {
            _loginButton.Click();
        }
    }
}
