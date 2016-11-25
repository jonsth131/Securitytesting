using OpenQA.Selenium;

namespace Securitytesting.Pages
{
    public abstract class PageObject
    {
        protected readonly IWebDriver Driver;

        protected PageObject(IWebDriver webDriver)
        {
            Driver = webDriver;
        }
    }
}